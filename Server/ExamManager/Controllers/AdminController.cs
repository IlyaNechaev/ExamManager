using ExamManager.Extensions;
using ExamManager.Models;
using AutoMapper;
using ExamManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExamManager.Filters;

namespace ExamManager.Controllers
{

    [Route("admin")]
    [Authorize]
    [NotDefaultUser("Home", nameof(HomeController.ChangeUserData), "pageId", 1)]
    public class AdminController : Controller
    {
        IUserService _userService { get; set; }
        IMapper _mapper { get; set; }
        IGroupService _groupService { get; set; }
        public AdminController(IUserService userService,
            IMapper mapper,
            IGroupService groupService)
        {
            _userService = userService;
            _mapper = mapper;
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.GetClaim(ClaimKey.Id));
            var user = await _userService.GetUser(userId);
            var userView = _mapper.Map<User, UserViewModel>(user);
            
            return View(userView);
        }

        [HttpPost("group/create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            var createdGroup = new Group();
            try
            {
                createdGroup = await _groupService.CreateGroup(request.GroupName);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

            if (createdGroup is null)
            {
                return Ok("Не удалось создать группу");
            }

            return Ok(createdGroup);
        }
    
        [HttpPost("group/add-students")]
        public async Task<IActionResult> CreateGroupStudents([FromBody] CreateGroupStudentsRequest request)
        {
            var studentAccounts = new List<UserViewModel>(request.Students.Count);
            if (request.Students is not null)
            {
                foreach (var student in request.Students)
                {
                    var user = await _userService.AddUser(student);
                    var addStudentTask = _groupService.AddStudent(request.GroupId, user.ObjectID);
                    studentAccounts.Add(_mapper.Map<User, UserViewModel>(user));

                    await addStudentTask;
                }
            }
            else if (request.File is not null)
            {
                Ok("Данная функция не доступна в текущей версии");
            }
            else
            {
                Ok("Не удалось добавить студентов");
            }

            return Ok("Студенты добавлены");
        }
    }
}
