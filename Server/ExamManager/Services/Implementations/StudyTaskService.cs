﻿using ExamManager.DAO;
using ExamManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ExamManager.Services;


public class StudyTaskService : IStudyTaskService
{
    ExamManagerDBContext _dbContext;

    public StudyTaskService(
        ExamManagerDBContext context
        )
    {
        _dbContext = context;
    }

    public async Task<StudyTask> CreateStudyTaskAsync(string title, string description, int virtualMachine)
    {
        var random = new Random();
        var task = new StudyTask
        {
            Title = title,
            Description = description,
            Number = (ushort)random.Next(100, 100_000),
            VirtualMachineID = virtualMachine
        };

        await _dbContext.Tasks.AddAsync(task);
        await _dbContext.SaveChangesAsync();

        return task;
    }
    public async Task AddStudyTaskAsync(StudyTask task)
    {
        await _dbContext.Tasks.AddAsync(task);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteStudyTaskAsync(Guid taskId)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(task => task.ObjectID == taskId);
        var personalTasks = _dbContext.UserTasks.Where(pTask => pTask.TaskID == task.ObjectID);
        // Удалить связанные задания у пользователей
        _dbContext.UserTasks.RemoveRange(personalTasks);

        _dbContext.Tasks.Remove(task ?? throw new InvalidDataException($"Задания {taskId} не существует"));
        await _dbContext.SaveChangesAsync();
    }

    public async Task<StudyTask> GetStudyTaskAsync(Guid taskId)
    {
        var task = await _dbContext.Tasks
            .AsNoTracking()
            .Include(nameof(StudyTask.PersonalTasks))
            .FirstOrDefaultAsync(task => task.ObjectID == taskId);

        return task ?? throw new InvalidDataException($"Задания {taskId} не существует");
    }

    public async Task<IEnumerable<StudyTask>> GetStudyTasksAsync(params Guid[] taskIds)
    {
        var tasks = await _dbContext.Tasks
            .Where(task => taskIds.Contains(task.ObjectID))
            .ToListAsync();

        return tasks;
    }

    public async Task<IEnumerable<StudyTask>> GetStudyTasksAsync(StudyTaskOptions options)
    {
        var query = $"SELECT * FROM {nameof(ExamManagerDBContext.Tasks)}";
        var conditions = GetQueryConditions(options);

        query += conditions.Condition;

        var tasks = await _dbContext.Tasks
            .FromSqlRaw(query, conditions.Parameters)
            .AsNoTracking()
            .ToListAsync();

        return tasks;
    }

    public Task<StudyTask> ModifyTaskAsync(Guid taskId, StudyTask task)
    {
        throw new NotImplementedException();
    }

    private (string Condition, SqlParameter[] Parameters) GetQueryConditions(StudyTaskOptions options)
    {
        var conditions = new List<(string Condition, SqlParameter Parameter)>(5);

        #region CREATE_CONDITIONS

        if (options.Title is not null)
        {
            conditions.Add((
                $"LOWER({nameof(StudyTask.Title)}) LIKE @title", 
                new SqlParameter("title", options.Title.ToLower())
                ));
        }
        if (options.Number is not null)
        {
            conditions.Add((
                $"{nameof(StudyTask.Number)} = @number",
                new SqlParameter("number", options.Number)
                ));
        }
        if (options.StudentIds is not null)
        {
            conditions.Add((
                $"EXISTS (SELECT 1 FROM {nameof(ExamManagerDBContext.UserTasks)} ut WHERE ut.{nameof(PersonalTask.TaskID)} = ObjectID AND ut.{nameof(PersonalTask.StudentID)} IN (@students))", 
                new SqlParameter("students", string.Join(", ", options.StudentIds))
                ));
        }

        #endregion

        if (conditions.Count > 0)
        {
            return ($"WHERE {string.Join(" AND ", conditions.Select(cond => cond.Condition))}", conditions.Select(cond => cond.Parameter).ToArray());
        }

        return (string.Empty, new SqlParameter[0]);
    }

}