using ExamManager.Models;
using AutoMapper;
using ExamManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExamManager.Models.RequestModels;
using ATMApplication.Filters;
using ExamManager.Filters;

namespace ExamManager.Controllers
{

    [Route(Routes.Group)]
    [ApiController]
    [JwtAuthorize]
    public class GroupController : ControllerBase
    {
        IUserService _userService { get; set; }
        IMapper _mapper { get; set; }
        IGroupService _groupService { get; set; }
        public GroupController(IUserService userService,
            IMapper mapper,
            IGroupService groupService)
        {
            _userService = userService;
            _mapper = mapper;
            _groupService = groupService;
        }

        [HttpGet(Routes.GetGroup)]
        [ValidateGuidFormat("id")]
        public async Task<IActionResult> GetGroup(string id)
        {
            var groupId = Guid.Parse(id);

            var group = await _groupService.GetGroup(groupId);

            return Ok(ResponseFactory.CreateResponse(group));
        }

        [HttpPost(Routes.CreateGroup)]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            var createdGroup = new Group();
            try
            {
                createdGroup = await _groupService.CreateGroup(request.GroupName);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse(createdGroup));
        }

        [HttpGet(Routes.GetGroupStudents)]
        [ValidateGuidFormat("id")]
        public async Task<IActionResult> GetGroupStudents(string id)
        {
            var groupId = Guid.Parse(id);
            var group = await _groupService.GetGroup(groupId, true);

            return Ok(ResponseFactory.CreateResponse(group.Students, group.Name));
        }

        [HttpPost(Routes.AddGroupStudent)]
        public async Task<IActionResult> AddGroupStudents([FromBody] AddStudentsRequest request)
        {
            var result = Parallel.ForEach(request.students, (student, token) =>
            {
                _groupService.AddStudent(request.groupId, student.id);
            });

            if (!result.IsCompleted)
            {
                return Ok(ResponseFactory.CreateResponse(new Exception("Не удалось добавить студентов")));
            }
            var group = await _groupService.GetGroup(request.groupId, true);

            return Ok(ResponseFactory.CreateResponse(group.Students, group.Name));
        }

        [HttpPost(Routes.RemoveGroupStudent)]
        public async Task<IActionResult> RemoveGroupStudents([FromBody] RemoveStudentsRequest request)
        {
            var result = Parallel.ForEach(request.students, (student, token) =>
            {
                _groupService.RemoveStudent(request.groupId, student.studentId);
            });

            if (!result.IsCompleted)
            {
                return Ok(ResponseFactory.CreateResponse(new Exception("Не удалось удалить студентов")));
            }
            var group = await _groupService.GetGroup(request.groupId, true);

            return Ok(ResponseFactory.CreateResponse(group.Students, group.Name));
        }
    }
}
