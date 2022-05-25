﻿using ExamManager.Models;
using ExamManager.Models.RequestModels;

namespace ExamManager.Services;

public static class RequestMapper
{
    static ISecurityService _securityService { get; set; }
    public static void AddRequestMapper(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        _securityService = serviceProvider.GetService<ISecurityService>();
    }

    public static StudyTask MapFrom(CreateTaskRequest request)
    {
        var task = new StudyTask
        {
            Title = request.title,
            Description = request.description,
            VirtualMachines = request.virtualMachines.Select(vm =>
            new VirtualMachineImage
            {
                ID = vm.id,
                Title = vm.title,
                Order = vm.order
            }).ToArray()
        };

        return task;
    }

    public static IEnumerable<User> MapFrom(CreateUsersRequest request)
    {
        var users = request.users.Select(u => new User
        {
            FirstName = u.firstName,
            LastName = u.lastName,
            Login = u.login,
            PasswordHash = _securityService.Encrypt(u.password),
            Role = u.role,
            StudentGroupID = u.groupId
        });

        return users;
    }

    public static string MapFrom(CreateGroupRequest request)
    {
        return request.name;
    }

    public static (Guid TaskID,StudyTask NewTask) MapFrom(ModifyTaskRequest request)
    {
        var newTask = new StudyTask
        {
            Title = request.title,
            Description = request.description
        };

        return (request.taskId, newTask);
    }

    public static IEnumerable<Guid> MapFrom(AddPersonalTasksRequest request)
    {
        return request.tasks?.Select(task => task.id);
    }
}