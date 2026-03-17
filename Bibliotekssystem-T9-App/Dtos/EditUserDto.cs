using System.ComponentModel.DataAnnotations;

namespace Bibliotekssystem_T9_App.Dtos
{
    public class EditUserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-post är obligatorisk")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; }

        [Required(ErrorMessage = "Roll måste väljas")]
        public string Role { get; set; } = "User";
    }
}