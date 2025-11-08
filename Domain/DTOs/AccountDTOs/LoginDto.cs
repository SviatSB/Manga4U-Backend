using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.AccountDTOs
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
