namespace Services.DTOs.ModelsDTOs
{
    public class CommentDto
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; } = null!;
        public long UserId { get; set; }

        // optional: id of the comment this is a reply to
        public long? RepliedCommentId { get; set; }

        // chapter external id (mangadex chapter id)
        public string ChapterExternalId { get; set; } = null!;

        // Optional: include user display info
        public string? UserNickname { get; set; }
        public string? UserAvatarUrl { get; set; }

        // legacy fields left out: MangaId / MangaName are not used here
    }
}
