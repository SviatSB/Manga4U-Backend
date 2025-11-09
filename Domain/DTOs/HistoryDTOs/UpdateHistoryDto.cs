using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.HistoryDTOs
{
    public class UpdateHistoryDto
    {
        [Required]
        public string MangaExternalId { get; set; } = null!;
        [Required]
        public string LastChapterId { get; set; } = null!;
        [Required]
        public string Language { get; set; } = null!;
        [Required]
        public string LastChapterTitle { get; set; } = null!;
        [Required]
        public int LastChapterNumber { get; set; }
    }
}
