using System.ComponentModel.DataAnnotations;

namespace Services.DTOs.AccountDTOs
{
    public class RegistrationDto
    {
        [Required]
        public string Login { get; set; } = null!;

        [Required]
        [MinLength(8)]

        public string Password { get; set; } = null!;

        [MinLength(3)]
        [MaxLength(16)]
        public string? Nickname { get; set; }
    }
}
