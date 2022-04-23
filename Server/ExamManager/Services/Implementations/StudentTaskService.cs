using ExamManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamManager.Services.Implementations;


public class StudentTaskService : IStudentTaskService
{
    DbContext _dbContext;
    public StudentTaskService(DbContext context)
    {
        _dbContext = context;
    }

    public async Task<StudentTask> ChangeStudentTaskStatus(Guid taskId, StudentTask.TaskStatus status)
    {
        var StudentTaskSet = _dbContext.Set<StudentTask>();

        var studentTask = await StudentTaskSet.FirstOrDefaultAsync(t => t.ObjectID == taskId);

        studentTask.Status = status;
        //StudentTaskSet.Update(studentTask);
        await _dbContext.SaveChangesAsync();

        return studentTask;
    }

    public async Task<StudentTask> CreateStudentTask(StudentTask task)
    {
        throw new NotImplementedException();
    }

    public Task<StudentTask> CreateStudentTask(string title, string description, string url, Guid authorId, Guid studentId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteStudentTask(Guid taskId)
    {
        throw new NotImplementedException();
    }

    public Task<StudentTask> GetStudentTask(Guid taskId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<StudentTask>> GetStudentTasks(Guid studentId)
    {
        throw new NotImplementedException();
    }

    public Task<StudentTask> UpdateStudentTask(StudentTask newTask, Guid? taskId = null)
    {
        throw new NotImplementedException();
    }
}