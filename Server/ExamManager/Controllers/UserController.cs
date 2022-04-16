using ExamManager.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ExamManager.Models;
using ExamManager.Models.Response;
using Microsoft.AspNetCore.Authorization;

namespace ExamManager.Controllers
{
    [Route("user")]
    [Authorize]
    public class UserController : Controller
    {
        IUserService _userService { get; set; }
        IMapper _mapper { get; set; }
        public UserController(IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var user = await _userService.GetUser(Guid.Parse(id));

            var userView = _mapper.Map<User, UserViewModel>(user);

            return Ok(ResponseFactory.CreateResponse(userView));
        }
    }
}
