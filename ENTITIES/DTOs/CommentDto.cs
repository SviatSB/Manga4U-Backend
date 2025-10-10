using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTOs
{
    public class CommentDto
    {
        public ulong Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; } = null!;
        public int UserId { get; set; }
        public int MangaId { get; set; }
        
        // Optional: Include user and manga info for display purposes
        public string? UserNickname { get; set; }
        public string? MangaName { get; set; }
    }
}
