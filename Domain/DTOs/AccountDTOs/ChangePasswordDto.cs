using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.AccountDTOs
{
    public class ChangePasswordDto
    {
        [Required]
        [MinLength(8)]
        public string OldPassword { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = null!;
    }
}
