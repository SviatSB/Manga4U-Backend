using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public class JwtConfig
    {
        [Required]
        public string Key { get; set; } = null!;
        [Required]
        public string Issuer { get; set; } = null!;
        [Required]
        public string Audience { get; set; } = null!;
        [Required]
        public int ExpireMinutes { get; set; }
    }
}
