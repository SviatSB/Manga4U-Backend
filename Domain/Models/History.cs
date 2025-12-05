namespace Domain.Models
{
    public class History : EntityBase
    {
        public string LastChapterId { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string LastChapterTitle { get; set; } = null!;
        public int LastChapterNumber { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; //not null | has default

        public string MangaExternalId { get; set; } = null!;

        //FK and References
        public long UserId { get; set; }  //not null
        public long MangaId { get; set; }  //not null

        public User User { get; set; } = null!;
        public Manga Manga { get; set; } = null!;
    }
}
