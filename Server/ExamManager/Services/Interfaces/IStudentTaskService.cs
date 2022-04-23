using ExamManager.Models;

namespace ExamManager.Services;

public interface IStudentTaskService
{
    public Task<StudentTask> GetStudentTask(Guid taskId);
    public Task<IEnumerable<StudentTask>> GetStudentTasks(Guid studentId);
    public Task<StudentTask> CreateStudentTask(StudentTask task);
    public Task<StudentTask> CreateStudentTask(string title, string description, string url, Guid authorId, Guid studentId);
    public Task<StudentTask> ChangeStudentTaskStatus(Guid taskId, StudentTask.TaskStatus status);
    public Task<StudentTask> UpdateStudentTask(StudentTask newTask, Guid? taskId = null);
    public Task DeleteStudentTask(Guid taskId);
}