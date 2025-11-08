namespace Domain.Models
{
    public class Tag
    {
        public long Id { get; set; }  // PK
        public string Name { get; set; } = null!; //not null
        public string TagExternalId { get; set; } = null!; //not null

        //FK and References
        public ICollection<Manga> Mangas { get; set; } = new List<Manga>();
    }
}
