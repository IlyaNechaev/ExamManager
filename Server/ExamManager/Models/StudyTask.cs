using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamManager.Models;

public record StudyTask
{
    [Key]
    public Guid ObjectID { get; set; }

    /// <summary>
    /// Название задания
    /// </summary>
    [Required]
    public string? Title { get; set; }
    /// <summary>
    /// Описание задания
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Внутренний номер задания
    /// </summary>
    [Required]
    public ushort? Number { get; set; }
    /// <summary>
    /// Идентификатор виртуальной машины
    /// </summary>
    [Required]
    public string? VirtualMachineID { get; set; }

    public IEnumerable<PersonalTask> PersonalTasks { get; set; }
}