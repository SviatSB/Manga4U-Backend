using System.ComponentModel.DataAnnotations;

namespace Services.DTOs.HistoryDTOs
{
    public class UpdateHistoryDto
    {
        public string MangaExternalId { get; set; } = null!;
        public string LastChapterId { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string LastChapterTitle { get; set; } = null!;
        public int LastChapterNumber { get; set; }
    }
}
