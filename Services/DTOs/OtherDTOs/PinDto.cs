using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.OtherDTOs
{
    public class PinDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public bool State { get; set; }
    }
}
