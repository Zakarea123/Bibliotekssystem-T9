namespace Bibliotekssystem_T9_App.Dtos;

// DTO som skickas från MVC-klienten till UserService vid inloggning
public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}