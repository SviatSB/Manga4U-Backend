using Domain.Models;

namespace Services.DTOs.ModelsDTOs
{
    public class CollectionDto
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; }
        public SystemCollectionType? SystemCollectionType { get; set; }
        public long UserId { get; set; }

        // Optional: Include user info and manga list for display purposes
        public string? UserNickname { get; set; }
        public List<MangaDto>? Mangas { get; set; }
        public int MangaCount { get; set; }
    }
}
