namespace Services.DTOs.CommentDTOs
{
    public class CommentPagedDto
    {
        public int TotalCount { get; set; }
        public int ReplyCount { get; set; }
        public List<CommentDto> Items { get; set; } = new List<CommentDto>();
    }
}
