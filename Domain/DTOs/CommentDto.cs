namespace Domain.DTOs
{
    public class CommentDto
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; } = null!;
        public long UserId { get; set; }
        public long MangaId { get; set; }

        // Optional: Include user and manga info for display purposes
        public string? UserNickname { get; set; }
        public string? MangaName { get; set; }
    }
}
