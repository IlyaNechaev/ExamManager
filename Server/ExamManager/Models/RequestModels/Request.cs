namespace ExamManager.Models.RequestModels;

public struct CreateGroupRequest
{
    public string GroupName { get; set; }
}

public struct CreateStudentsRequest
{
    public Guid GroupId { get; set; }
    public List<RegisterEditModel> Students { get; set; }
}
