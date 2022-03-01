using ExamManager.Models;
using System.Security.Claims;

namespace ExamManager.Services;

public interface IUserService
{
    public Task<User> AddUser(RegisterEditModel model);
    public Task<User> GetUser(Guid userId);
    public Task<User> GetUser(string login, string password);
    public Task<ClaimsPrincipal> CreateUserPrincipal(User user);

    /// <summary>
    /// Меняет значения свойств пользователя по его ObjectID
    /// </summary>
    /// <param name="userId">ObjectID пользователя</param>
    /// <param name="data">Массив свойств, которые необходимо изменить</param>
    /// <returns></returns>
    public Task<ValidationResult> ChangeUserData(Guid userId, params Property[] data);
}

public class CreateUserOptions
{
    public User UserToCreate { get; set; }
    public bool GenerateLogin { get; set; } = false;
    public bool GeneratePassword { get; set; } = false;
    public bool CheckUniqueLogin { get; set; } = true;
}