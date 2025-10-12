using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTOs.AccountDTOs
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
