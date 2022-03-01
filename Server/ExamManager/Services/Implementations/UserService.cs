using AutoMapper;
using ExamManager.Extensions;
using ExamManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace ExamManager.Services;

public class UserService : IUserService
{
    DbContext _dbContext { get; init; }
    ISecurityService _securityService { get; init; }
    IMapper _mapper { get; set; }

    public UserService(DbContext dbContext,
        ISecurityService securityService,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _securityService = securityService;
        _mapper = mapper;
    }


    public async Task<User> AddUser(RegisterEditModel model)
    {
        var UserSet = _dbContext.Set<User>();
        var random = new Random();

        if (model.Login is null)
        {
            do
            {
                model.Login = $"{model.FirstName}{random.Next(100, 999)}";
            }
            while (!await UserSet.AnyAsync(u => u.Login == model.Login));
        }

        if (model.Password is null)
        {
            model.Password = _securityService.GeneratePassword(10);
        }

        var user = _mapper.Map<RegisterEditModel, User>(model);

        await UserSet.AddAsync(user);

        return user;
    }

    public async Task<ClaimsPrincipal?> CreateUserPrincipal(User user)
    {
        if (user is null)
            return null;

        var claims = new Claim[]
        {
            new Claim(ClaimKey.Login, user.Login),
            new Claim(ClaimKey.Role, user.Role.ToString()),
            new Claim(ClaimKey.Id, user.ObjectID.ToString())
        };

        // Создаем объект ClaimsIdentity
        var claimId = new ClaimsIdentity(claims, "AppCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        return await Task.FromResult(new ClaimsPrincipal(claimId));
    }

    public async Task<User?> GetUser(Guid userId)
    {
        var UserSet = _dbContext.Set<User>();

        var user = await UserSet.FirstOrDefaultAsync(user => user.ObjectID == userId);

        return user;
    }

    public async Task<User> GetUser(string login, string password)
    {
        var UserSet = _dbContext.Set<User>();
        var passwordHash = _securityService.Encrypt(password);

        var user = await UserSet.FirstOrDefaultAsync(user => user.Login == login && user.PasswordHash.Equals(passwordHash));

        return user;
    }

    public async Task<ValidationResult> ChangeUserData(Guid userId, params Property[] data)
    {
        var UserSet = _dbContext.Set<User>();
        var user = await UserSet.FirstOrDefaultAsync(user => user.ObjectID == userId);
        var result = new ValidationResult();

        if (user is null)
        {
            result.AddCommonMessage("Пользователь не найден");
            return result;
        }

        var entityManager = new EntityManager();
        // Создаем временного пользователя и копируем значения свойств из user
        var tempUser = new User();
        tempUser = entityManager.CopyInto(tempUser).AllPropertiesFrom(user).GetResult();

        // Для изменяемых свойств временного пользователя присваиваем переданные значения
        var userCopyManager = entityManager.CopyInto(tempUser);
        foreach (var prop in data)
        {
            userCopyManager.Property(prop.Name, prop.Value);
        }
        tempUser = userCopyManager.GetResult();

        // Если в изменяемых параметрах присутствует логин
        if (data?.Select(field => field.Name).Contains(nameof(Models.User.Login)) ?? false)
        {
            result = await ValidateRegisterUserLogin(tempUser);
        }

        if (!result.HasErrors)
        {
            user = entityManager.CopyInto(user).AllPropertiesFrom(tempUser).GetResult();
            UserSet.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        return result;

        async Task<ValidationResult> ValidateRegisterUserLogin(User userValidation)
        {
            if (await UserSet.AnyAsync(user => user.Login.Equals(userValidation.GetLogin())))
            {
                result.AddMessage(nameof(User.Login), "Пользователь с таким логином уже существует");
            }

            return result;
        }
    }

    private void CopyFields(User source, User target)
    {
        var userType = typeof(User);
        foreach(var property in userType.GetProperties())
        {
            property.SetValue(target, property.GetValue(source));
        }
    }
}
