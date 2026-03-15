namespace Bibliotekssystem_T9_App.Dtos;

// DTO för användare som hämtas från UserService API
public class UserDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}