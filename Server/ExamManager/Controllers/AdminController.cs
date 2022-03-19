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

        [HttpGet("/create-group")]
        public async Task<IActionResult> CreateGroup(string name)
        {
            var createdGroup = new Group();
            try
            {
                createdGroup = await _groupService.CreateGroup(name);
            }
            catch (Exception ex)
            {
                return Ok(new { Message = ex.Message });
            }

            if (createdGroup is null)
            {
                return Ok(new { Message = "Не удалось создать группу"  });
            }

            return Ok(createdGroup);
        }

        [HttpGet("/get-groups")]
        public async Task<IActionResult> GetGroups(string name)
        {
            var temp = (await _groupService.GetGroups(name)).ToList();
            var groups = (await _groupService.GetGroups(name))
                .Select(g => new { g.ObjectID, g.Name });
            return Ok(groups);
        }

        [HttpGet("/get-group")]
        public async Task<IActionResult> GetGroups(string? name, string? id)
        {
            Group group = null;
            if (id is not null)
            {
                try
                {
                    group = await _groupService.GetGroup(Guid.Parse(id));
                }
                catch { }
            }
            else if (name is not null && group is null)
            {
                try
                {
                    group = await _groupService.GetGroup(name);
                }
                catch { }
            }

            return Ok(group);
        }
    }
}
