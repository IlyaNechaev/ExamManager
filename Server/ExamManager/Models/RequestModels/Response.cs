﻿using System.Net;

namespace ExamManager.Models.Response;

public record Response
{
    public HttpStatusCode status { get; set; }
    public string type { get => GetType().Name; }
}

public record JWTResponse : Response
{
    public string token { get; set; }
}

public record UserDataResponse : Response
{
    public Guid id{ get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public UserRole role { get; set; }
}

public record UsersDataResponse : Response
{
    public List<UserView> users { get; set; }
    public struct UserView
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string groupName { get; set; }
    }
}

public record TasksDataResponse : Response
{
    public TaskView[] tasks { get; set; }
    public struct TaskView
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public TaskStatus status { get; set; }
        public Guid studentId { get; set; }
    }
}

public record TaskDataResponse : Response
{
    public Guid id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public TaskStatus status { get; set; }
    public Guid authorId { get; set; }
    public string url { get; set; }
}

public record GroupDataResponse : Response
{
    public Guid id { get; set; }
    public string name { get; set; }
    public int studentsCount { get; set; }
}

public record BadResponse : Response
{
    public Dictionary<string, List<string>>? errors { get; set; }
}

public record ExceptionResponse : Response
{
    public string exceptionType { get; set; }
    public string message { get; set; }
    public string stackTrace { get; set; }
}