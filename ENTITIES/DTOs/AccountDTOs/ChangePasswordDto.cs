using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTOs.AccountDTOs
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
