using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamManager.Models;

public record VirtualMachine
{
    [Key]
    public Guid ObjectID { get; set; }

    public string ID { get; set; }

    [ForeignKey(nameof(OwnerID))]
    public User Owner { get; set; }
    public Guid OwnerID { get; set; }

    [ForeignKey(nameof(TaskID))]
    public PersonalTask Task { get; set; }
    public Guid TaskID { get; set; }
}