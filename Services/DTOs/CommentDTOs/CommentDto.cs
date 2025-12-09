namespace Services.DTOs.CommentDTOs
{
    public class CommentDto
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; } = null!;
        public long UserId { get; set; }
        public long? RepliedCommentId { get; set; }
        public string ChapterExternalId { get; set; } = null!;
        public string? UserNickname { get; set; }
        public string? UserAvatarUrl { get; set; }
        public bool IsPined { get; set; }
        public int ReplyCount { get; set; }
    }
}
