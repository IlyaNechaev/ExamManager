using ExamManager.Models;

namespace ExamManager.Services;

public interface IStudyTaskService
{
    public Task<StudyTask> GetStudyTaskAsync(Guid taskId);
    public Task<IEnumerable<StudyTask>> GetStudyTasksAsync(params Guid[] taskIds);
    public Task<IEnumerable<StudyTask>> GetStudyTasksAsync(StudyTaskOptions options);
    public Task<StudyTask> CreateStudyTaskAsync(string title, string description, VirtualMachineImage[] virtualMachines);
    public Task CreateStudyTaskAsync(StudyTask task);
    public Task DeleteStudyTaskAsync(Guid taskId);

    /// <summary>
    /// Изменяет значения полей сущности <see cref="StudyTask"/>
    /// </summary>
    /// <param name="task">Объект, значения свойств которого будут изменены у имеющегося в БД (свойства со значением null не будут изменены)</param>
    public Task<StudyTask> ModifyTaskAsync(Guid taskId, StudyTask task);
    public Task AssignTaskToStudentAsync(Guid taskId, Guid studentId);
}

public struct StudyTaskOptions
{
    public Guid[]? StudentIds { get; set; }
    public string? Title { get; set; }
    public ushort? Number { get; set; }
}