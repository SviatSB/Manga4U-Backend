using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.HistoryDTOs
{
    public class GetHistoryDto
    {
        public string MangaName { get; set; } = null!;
        public string MangaExternalId { get; set; } = null!;
        public string LastChapterId { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string LastChapterTitle { get; set; } = null!;
        public int LastChapterNumber { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
