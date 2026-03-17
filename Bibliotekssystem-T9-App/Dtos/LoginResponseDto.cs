namespace Bibliotekssystem_T9_App.Dtos;

// DTO som tas emot från UserService efter lyckad inloggning
public class LoginResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}