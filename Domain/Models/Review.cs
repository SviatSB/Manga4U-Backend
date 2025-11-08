using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Review
    {
        //TODO unique для (user,manga)

        public long Id { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }  // 1-5
        public string? Text { get; set; } = null; //nullable | has default = null;
        public DateTime CreationTime { get; set; } = DateTime.Now; //notn null | has default


        //FK and References
        public long UserId { get; set; }  //not null
        public long MangaId { get; set; }  //not null

        public User User { get; set; } = null!;
        public Manga Manga { get; set; } = null!;
    }
}
