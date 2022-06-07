﻿using ATMApplication.Filters;
using ExamManager.Models;
using ExamManager.Models.RequestModels;
using ExamManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

        [HttpPost(Routes.GetTasks)]
        public async Task<IActionResult> GetTasks([FromBody] GetTasksRequest request)
        {
            var options = RequestMapper.MapFrom(request);
            var tasks = await _taskService.GetStudyTasksAsync(options);

            return Ok(ResponseFactory.CreateResponse(tasks));
        }

        [HttpPost(Routes.CreateTask)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            (var studyTask, var studentIds) = RequestMapper.MapFrom(request);
            try
            {
                studyTask = await _taskService.CreateStudyTaskAsync(studyTask.Title, studyTask.Description!, studyTask.VirtualMachines?.ToArray());

                if (studentIds is not null)
                {
                    foreach (var id in studentIds)
                    {
                        await _taskService.AssignTaskToStudentAsync(studyTask.ObjectID, id);
                    }
                }

                studyTask = await _taskService.GetStudyTaskAsync(studyTask.ObjectID);
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
            try
            {
                await _taskService.DeleteStudyTaskAsync(request.taskId);
            }
            catch(Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse());
        }

        [HttpPost(Routes.ModifyTask)]
        public async Task<IActionResult> ModifyTask([FromBody] ModifyTaskRequest request)
        {
            (var taskId, var newTask, var studentIds) = RequestMapper.MapFrom(request);

            var studentTask = await _taskService.ModifyTaskAsync(taskId, newTask);

            if (studentIds is not null)
            {
                var students = await _taskService.GetStudentsHavingTask(taskId);

                var studentsToRemove = students.Where(s => !request.students!.Contains(s));
                var studentsToAdd = request.students!.Where(s => !students.Contains(s));

                if (studentsToRemove.Any())
                {
                    foreach (var student in studentsToRemove)
                    {
                        await _taskService.WithdrawTaskFromStudentAsync(taskId, student);
                    }                    
                }

                if (studentsToAdd.Any())
                {
                    foreach (var student in studentsToAdd)
                    {
                        await _taskService.AssignTaskToStudentAsync(taskId, student);
                    }                    
                }
            }
            studentTask = await _taskService.GetStudyTaskAsync(studentTask.ObjectID);

            return Ok(ResponseFactory.CreateResponse(studentTask));
        }

        [HttpGet(Routes.StartTask)]
        [ValidateGuidFormat("taskId")]
        public async Task<IActionResult> StartTask(string taskId, string id)
        {
            var currentUserID = ((User)HttpContext.Items["User"]!).ObjectID;
            var currentTaskId = Guid.Parse(taskId);

            var vmId = string.Empty;
            try
            {
                vmId = await _taskService.StartTaskVirtualMachine(id, currentTaskId, currentUserID);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse(vmId, System.Net.HttpStatusCode.OK));
        }

        [HttpGet(Routes.GetTaskStatus)]
        [ValidateGuidFormat("taskId")]
        public async Task<IActionResult> GetTaskStatus(string taskId, string id)
        {
            var pTaskId = Guid.Parse(taskId);
            IEnumerable<VirtualMachine> vMachines;

            try
            {
                vMachines = await _taskService.GetPersonalTaskVirtualMachinesAsync(pTaskId);
            }
            catch (DataNotFoundException<PersonalTask> ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex.WithMessage($"Не удалось найти задание {taskId}")));
            }
            catch (DataNotFoundException<VirtualMachine> ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex.WithMessage($"Не удалось найти виртуальные машины для задания {taskId}")));
            }

            var vMachine = vMachines!.FirstOrDefault(vm => vm.Name == id);
            if (vMachine is null)
            {
                return Ok(ResponseFactory.CreateResponse(new Exception($"У задания {taskId} нет виртуальной машины {id}")));
            }

            return Ok(ResponseFactory.CreateResponse(vMachine.Status));
        }

        [HttpGet(Routes.StopTask)]
        public async Task<IActionResult> StopTask(string id)
        {
            try
            {
                await _taskService.StopTaskVirtualMachine(id);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse());
        }

        [HttpGet(Routes.CheckTask)]
        [ValidateGuidFormat("taskId")]
        public async Task<IActionResult> CheckTak(string taskId)
        {
            try
            {
                await _taskService.CheckStudyTaskAsync(Guid.Parse(taskId));
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse());
        }

        [HttpGet(Routes.ConnectVirtualMachine)]
        public async Task<IActionResult> ConnectVirtualMachine(string taskId, string id)
        {
            var vMachine = await _virtualMachineService.GetVirtualMachine(id);
            if (vMachine is null)
            {
                return Ok(ResponseFactory.CreateResponse(new InvalidDataException($"Виртуальной машины {id} не существует")));
            }

            var fileText = new StringBuilder();

            fileText.AppendLine("ConnMethod=tcp;");
            fileText.AppendLine($"ConnTime={DateTime.Now};");
            fileText.AppendLine($"FriendlyName={vMachine.Name};");
            fileText.AppendLine($"Host={vMachine.Host}:{vMachine.Port};");
            fileText.AppendLine($"Password={vMachine.Password};");
            fileText.AppendLine($"RelativePtr=0;");
            fileText.AppendLine($"Uuid={vMachine.ObjectID};");

            return Ok(new { Text = fileText.ToString() });
        }
    
    }
}
