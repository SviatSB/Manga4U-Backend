using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Models
{
    public class Manga
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = null!; //not null

        //не знаю нужна ли тут такая инфа. Вобще можно даже без названия обходится
        //public string Author { get; set; }
        public string ExternalId { get; set; } = null!; //not null


        //FK and References
        public ICollection<History> Histories { get; set; } = new List<History>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}
