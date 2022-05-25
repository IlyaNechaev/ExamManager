﻿using ATMApplication.Filters;
using ExamManager.Models;
using ExamManager.Models.RequestModels;
using ExamManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamManager.Controllers
{
    [Route(Routes.Task)]
    public class StudyTasksController : Controller
    {
        IStudyTaskService _taskService { get; }
        IUserService _userService { get; }
        IVirtualMachineService _virtualMachineService { get; set; }

        public StudyTasksController(
            IStudyTaskService studentTaskService, IUserService userService, IVirtualMachineService virtualMachineService)
        {
            _taskService = studentTaskService;
            _userService = userService;
            _virtualMachineService = virtualMachineService;
        }

        [HttpGet(Routes.GetTask)]
        [ValidateGuidFormat("id")]
        public async Task<IActionResult> GetTask(string id)
        {
            var taskId = Guid.Parse(id);
            var studentTask = await _taskService.GetStudyTaskAsync(taskId);

            return Ok(ResponseFactory.CreateResponse(studentTask));
        }

        [HttpPost(Routes.CreateTask)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            var studyTask = RequestMapper.MapFrom(request);
            try
            {
                studyTask = await _taskService.CreateStudyTaskAsync(studyTask.Title!, studyTask.Description!, studyTask.VirtualMachines!);

                return Ok(ResponseFactory.CreateResponse(studyTask));
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

        }

        [HttpPost(Routes.DeleteTask)]
        public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskRequest request)
        {
            await _taskService.DeleteStudyTaskAsync(request.taskId);

            return Ok(ResponseFactory.CreateResponse());
        }

        [HttpPost(Routes.ModifyTask)]
        public async Task<IActionResult> ModifyTask([FromBody] ModifyTaskRequest request)
        {
            var studyTask = new StudyTask
            {
                Title = request.title,
                Description = request.description                
            };

            var studentTask = await _taskService.ModifyTaskAsync(request.taskId, studyTask);
            return Ok(ResponseFactory.CreateResponse(studentTask));
        }

        [HttpGet(Routes.StartVirtualMachine)]
        public async Task<IActionResult> StartVirtualMachine(string id)
        {
            var currentUserID = ((User)HttpContext.Items["User"]).ObjectID;

            var vmId = string.Empty;
            try
            {
                await _virtualMachineService.StartVirtualMachine(id, currentUserID);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse());
        }

        [HttpGet(Routes.StopVirtualMachine)]
        public async Task<IActionResult> StopVirtualMachine(string id)
        {
            var currentUserID = ((User)HttpContext.Items["User"]).ObjectID;

            try
            {
                await _virtualMachineService.StopVirtualMachine(id, currentUserID);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse());
        }
    }
}
