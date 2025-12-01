using System;

namespace Services.DTOs.ReviewDTOs
{
    public class ReviewResponseDto
    {
        public long Id { get; set; }
        public int Stars { get; set; }
        public string? Text { get; set; }
        public DateTime CreationTime { get; set; }
        public long UserId { get; set; }
        public string? UserNickname { get; set; }
        public string? UserAvatarUrl { get; set; }
    }
}
