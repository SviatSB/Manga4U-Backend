namespace Domain.Models
{
    public class History
    {
        public long Id { get; set; }
        public int LastChapter { get; set; } //not null | has default = 0?
        public int LastPage { get; set; } //not null | has default = 0?
        public DateTime UpdatedAt { get; set; } = DateTime.Now; //not null | has default


        //FK and References
        public long UserId { get; set; }  //not null
        public long MangaId { get; set; }  //not null

        public User User { get; set; } = null!;
        public Manga Manga { get; set; } = null!;
    }
}
