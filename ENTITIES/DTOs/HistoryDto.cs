using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTOs
{
    public class HistoryDto
    {
        public ulong Id { get; set; }
        public int LastChapter { get; set; }
        public int LastPage { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public int MangaId { get; set; }
        
        // Optional: Include user and manga info for display purposes
        public string? UserNickname { get; set; }
        public string? MangaName { get; set; }
    }
}
