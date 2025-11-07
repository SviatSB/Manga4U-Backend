namespace Domain.DTOs
{
    public class HistoryDto
    {
        public long Id { get; set; }
        public int LastChapter { get; set; }
        public int LastPage { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long UserId { get; set; }
        public long MangaId { get; set; }

        // Optional: Include user and manga info for display purposes
        public string? UserNickname { get; set; }
        public string? MangaName { get; set; }
    }
}
