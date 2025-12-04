namespace Services.DTOs.CommentDTOs
{
    public class CreateCommentDto
    {
        public string MangaChapterExternalId { get; set; } = null!;
        public string Text { get; set; } = null!;
        public long? ParentCommentId { get; set; }
    }
}
