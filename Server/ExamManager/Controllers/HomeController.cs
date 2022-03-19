﻿using ExamManager.Extensions;
using AutoMapper;
using ExamManager.Models;
using ExamManager.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ExamManager.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private IUserService _userService { get; set; }
        public IMapper _mapper { get; set; }
        SignInManager _signInManager { get; set; }

        public HomeController(ILogger<HomeController> logger,
            IUserService userService,
            IMapper mapper,
            SignInManager signInManager)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginEditModel model,
                                   [FromServices] IUserService userService)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ResponseFactory.CreateResponse(ModelState));
            }

            var user = await userService.GetUser(model.Login, model.Password);
            if (user is null)
            {
                ModelState.AddModelError("Default", "Пользователя с такими логином и паролем не существует");
                return Ok(ResponseFactory.CreateResponse(ModelState));
            }
            //var principal = await userService.CreateUserPrincipal(user);

            var token = string.Empty;
            try
            {
                token = _signInManager.LogIn().UsingJWT(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Default", ex.Message);
                return Ok(ResponseFactory.CreateResponse(ModelState));
            }

            return Ok(ResponseFactory.CreateResponse(token));
        }

        //[HttpGet("/change")]
        //public async Task<IActionResult> ChangeUserData(int pageId,
        //                                                [FromServices] IUserService userService)
        //{
        //    var userId = Guid.Parse(User.GetClaim(ClaimKey.Id));
        //    var user = await userService.GetUser(userId);

        //    var action = pageId switch
        //    {
        //        1 when !user.IsDefault => await RedirectAfterAuthorization(),
        //        2 when user.IsDefault => RedirectToAction(nameof(ChangeUserData), new { pageId = 1 }),
        //        > 2 => await RedirectAfterAuthorization(),
        //        _ => View()
        //    };

        //    return action;
        //}

        //[HttpPost("/change")]
        //public async Task<IActionResult> ChangeUserData(RegisterEditModel model,
        //                                                [FromQuery] int pageId,
        //                                                [FromServices] ISecurityService securityService)
        //{
        //    string[] fields = new string[0];

        //    if (pageId == 1)
        //    {
        //        fields = new string[]
        //        {
        //            nameof(RegisterEditModel.Login),
        //            nameof(RegisterEditModel.Password),
        //            nameof(RegisterEditModel.ConfirmPassword)
        //        };
        //    }
        //    else if (pageId == 2)
        //    {

        //        fields = new string[]
        //        {
        //            nameof(RegisterEditModel.FirstName),
        //            nameof(RegisterEditModel.MiddleName)
        //        };
        //    }

        //    if (!await ChangeUserData(model, fields))
        //    {
        //        return View(model);
        //    }

        //    return pageId switch
        //    {
        //        1 => RedirectToAction(nameof(HomeController.ChangeUserData), new { pageId = 2 }),
        //        2 => await RedirectAfterAuthorization()
        //    };
        //}

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.LogOut().UsingClaims(HttpContext);

            return Ok(ResponseFactory.CreateResponse());
        }

        //private async Task<IActionResult> RedirectAfterAuthorization()
        //{
        //    if (!Guid.TryParse(User.GetClaim(ClaimKey.Id), out var userId))
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    var user = await _userService.GetUser(userId);

        //    var action = user switch
        //    {
        //        null => throw new ArgumentNullException(),
        //        { IsDefault: true } => RedirectToAction(nameof(ChangeUserData), new { pageId = 1 }),
        //        { Role: UserRole.ADMIN } => RedirectToAction(nameof(AdminController.Index), "Admin"),
        //        { Role: UserRole.STUDENT } => RedirectToAction(nameof(AdminController.Index), "Student"),
        //        _ => throw new InvalidOperationException()
        //    };

        //    return action;
        //}

        //private async Task<bool> ChangeUserData(RegisterEditModel model, params string[] data)
        //{
        //    foreach (var errorField in ModelState.Keys)
        //    {
        //        if (!data.Contains(errorField))
        //        {
        //            ModelState.Remove(errorField);
        //        }
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return false;
        //    }

        //    var userId = Guid.Parse(User.GetClaim(ClaimKey.Id));
        //    var userType = typeof(User);
        //    var registeredUser = _mapper.Map<RegisterEditModel, User>(model);

        //    // Подготовка свойств
        //    #region DATA_PREPARATION

        //    //// Преобразование названий свойств из RegisterEditModel в User
        //    //var userFieldsDict = new Dictionary<string, string>()
        //    //{
        //    //    { nameof(RegisterEditModel.Password), nameof(Models.User.PasswordHash) },
        //    //    { nameof(RegisterEditModel.ConfirmPassword), null }
        //    //};

        //    //List<Property> propertiesData = data
        //    //    // Убираем свойства, которые не присутствуют в User (например, свойство, где повторно написан пароль)
        //    //    .Where(fieldName => !userFieldsDict.ContainsKey(fieldName) || !string.IsNullOrEmpty(userFieldsDict[fieldName]))
        //    //    .Select(fieldName =>
        //    //    {
        //    //        var name = userFieldsDict.ContainsKey(fieldName) ? userFieldsDict[fieldName] : fieldName;
        //    //        var value = userType.GetProperty(name)!.GetValue(registeredUser);
        //    //        return new Property { Name = name, Value = value };
        //    //    })
        //    //    .ToList()!;
        //    var mappingManager = new EntityManager().Map<RegisterEditModel, User>();
        //    var propertiesData = new List<Property>();

        //    foreach (var propertyName in data)
        //    {
        //        var property = new Property();
        //        property.Name = mappingManager.PropertyName(propertyName);

        //        if (string.IsNullOrEmpty(property.Name))
        //        {
        //            continue;
        //        }

        //        property.Value = userType.GetProperty(property.Name)!.GetValue(registeredUser);
        //        propertiesData.Add(property);
        //    }

        //    propertiesData.Add(
        //        new Property
        //        {
        //            Name = nameof(Models.User.IsDefault),
        //            Value = false
        //        });

        //    #endregion

        //    var validationResult = await _userService.ChangeUserData(userId, propertiesData.ToArray());

        //    if (validationResult.HasErrors)
        //    {
        //        ModelState.AddErrors(validationResult.ErrorMessages);
        //        return false;
        //    }

        //    return true;
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}