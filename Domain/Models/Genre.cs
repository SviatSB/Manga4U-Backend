namespace Domain.Models
{
    public class Genre
    {
        public long Id { get; set; }  // PK
        public string Name { get; set; } = null!; //not null


        //FK and References
        public ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();
    }
}
