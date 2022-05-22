using System.ComponentModel.DataAnnotations;

namespace ExamManager.Models;

public record VirtualMachineImage
{
    [Key]
    public Guid ObjectID { get; set; }

    [Required]
    public string ID { get; set; }

    [Required]
    public string Title { get; set; }

    public int? Order { get; set; }
}
