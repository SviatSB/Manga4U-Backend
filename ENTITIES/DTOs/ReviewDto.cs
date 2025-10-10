using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ENTITIES.DTOs
{
    public class ReviewDto
    {
        public ulong Id { get; set; }
        
        [Range(1, 5)]
        public int Stars { get; set; }
        public string? Text { get; set; }
        public DateTime CreationTime { get; set; }
        public int UserId { get; set; }
        public int MangaId { get; set; }
        
        // Optional: Include user and manga info for display purposes
        public string? UserNickname { get; set; }
        public string? MangaName { get; set; }
    }
}
