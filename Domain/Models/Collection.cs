namespace Domain.Models
{
    public class Collection : EntityBase
    {
        public DateTime CreationTime { get; set; } = DateTime.UtcNow; //not null | has default
        public string Name { get; set; } = null!;  //not null
        public bool IsPublic { get; set; } = false; //not null | has default
        public SystemCollectionType? SystemCollectionType { get; set; } = null; //nullable | has default

        //FK and References
        public long UserId { get; set; } //not null
        public User User { get; set; } = null!;

        public ICollection<Manga> Mangas { get; set; } = new List<Manga>();
    }

    public enum SystemCollectionType
    {
        Favorite,
        Reading,
        WantToRead,
        Completed,
    }
}
