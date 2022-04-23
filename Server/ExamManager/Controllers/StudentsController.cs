using AutoMapper;
using ExamManager.Extensions;
using ExamManager.Filters;
using ExamManager.Services;
using ExamManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExamManager.Models.RequestModels;

namespace ExamManager.Controllers
{
    [ApiController]
    [JwtAuthorize]
    public class StudentsController : ControllerBase
    {
        IUserService _userService { get; set; }
        IMapper _mapper { get; set; }
        public StudentsController(IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet(Routes.GetStudents)]
        public async Task<IActionResult> GetStudents([FromBody] GetStudentsRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost(Routes.CreateStudents)]
        public async Task<IActionResult> CreateStudents([FromBody] CreateStudentsRequest request)
        {
            throw new NotImplementedException();
        }

        [HttpPost(Routes.DeleteStudents)]
        public async Task<IActionResult> DeleteStudents([FromBody] DeleteStudentsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}