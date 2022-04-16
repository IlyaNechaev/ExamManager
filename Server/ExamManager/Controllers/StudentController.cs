using AutoMapper;
using ExamManager.Extensions;
using ExamManager.Filters;
using ExamManager.Services;
using ExamManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamManager.Controllers
{
    [Route("student")]
    [ApiController]
    [Authorize]
    //[NotDefaultUser("Home", nameof(HomeController.ChangeUserData), "pageId", 1)]
    public class StudentController : ControllerBase
    {
        IUserService _userService { get; set; }
        IMapper _mapper { get; set; }
        public StudentController(IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
    
    }
}