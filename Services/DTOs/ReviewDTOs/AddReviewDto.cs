using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.ReviewDTOs
{
    public class AddReviewDto
    {
        [Required]
        public string MangaExternalId { get; set; } = null!;
        [Range(1, 5)]
        [Required]
        public int Stars { get; set; }
        public string? Text { get; set; }
    }
}
