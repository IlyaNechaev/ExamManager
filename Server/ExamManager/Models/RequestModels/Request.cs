namespace ExamManager.Models.RequestModels;

public struct CreateTaskRequest
{
    public string title { get; set; }
    public string description { get; set; }
    public string url { get; set; }
    public Guid studentId { get; set; }
    public Guid authorId { get; set; }
}

public struct DeleteTaskRequest
{
    public Guid taskId { get; set; }
}

public struct ModifyTaskRequest
{
    public Guid taskId { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public Guid studentId { get; set; }
    public Guid authorId { get; set; }
    public TaskStatus status { get; set; }
}

public struct CreateGroupRequest
{
    public string GroupName { get; set; }
}

public struct AddStudentsRequest
{
    public Guid groupId { get; set; }
    public StudentView[] students { get; set; }
    public struct StudentView
    {
        public Guid id { get; set; }
    }
}

public struct RemoveStudentsRequest
{
    public Guid groupId { get; set; }
    public StudentView[] students { get; set; }

    public struct StudentView
    {
        public Guid studentId { get; set; }
        public Guid groupId { get; set; }
    }
}

public struct GetStudentsRequest
{
    public Guid groupId { get; set; }
    public TaskStatus taskStatus { get; set; }
}

public struct CreateStudentsRequest
{
    public Guid GroupId { get; set; }
    public List<RegisterEditModel> Students { get; set; }
}

public struct DeleteStudentsRequest
{
    public StudentView[] students { get; set; }
    public struct StudentView
    {
        public Guid id { get; set; }
        public bool onlyLogin { get; set; }
    }
}