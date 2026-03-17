using System.ComponentModel.DataAnnotations;

namespace Bibliotekssystem_T9_App.Dtos;

// DTO som används när admin skapar en ny användare via MVC-klienten
public class CreateUserDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [RegularExpression("Admin|User")]
    public string Role { get; set; } = "User";
}