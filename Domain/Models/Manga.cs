namespace Domain.Models
{
    public class Manga
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!; //not null

        //не знаю нужна ли тут такая инфа. Вобще можно даже без названия обходится
        //public string Author { get; set; }
        public string ExternalId { get; set; } = null!; //not null


        //FK and References
        public ICollection<History> Histories { get; set; } = new List<History>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}
