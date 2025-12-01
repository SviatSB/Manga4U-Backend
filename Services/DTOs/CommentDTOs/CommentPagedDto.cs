using System.Collections.Generic;

namespace Services.DTOs.CommentDTOs
{
    public class CommentPagedDto
    {
        public int TotalCount { get; set; }
        public int ReplyCount { get; set; }
        public List<ModelsDTOs.CommentDto> Items { get; set; } = new List<ModelsDTOs.CommentDto>();
    }
}
