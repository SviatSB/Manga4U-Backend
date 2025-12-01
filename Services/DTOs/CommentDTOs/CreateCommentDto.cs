using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.CommentDTOs
{
    public class CreateCommentDto
    {
        public string MangaChapterExternalId { get; set; } = null!;
        public string Text { get; set; } = null!;
        public long? ParentCommentId { get; set; }
    }
}
