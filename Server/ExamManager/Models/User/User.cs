using System.ComponentModel.DataAnnotations;

namespace ExamManager.Models;

public class User : IUserValidationModel
{
    [Key]
    public Guid ObjectID { get; set; }

    [Required(ErrorMessage = "Введите логин")]
    public string Login { get; set; }
    [Required(ErrorMessage = "Введите пароль")]
    public string PasswordHash { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public UserRole Role { get; set; }

    public bool IsDefault { get; set; } = true;
    public Group? StudentGroup { get; set; }

    public string GetFirstName() => FirstName;
    public string GetLogin() => Login;
    public string GetLastName() => LastName;
    public Guid GetObjectID() => ObjectID;
}

public enum UserRole
{
    ADMIN = 0,
    STUDENT = 1 << 0
}