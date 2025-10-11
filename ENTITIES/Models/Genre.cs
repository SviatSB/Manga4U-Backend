using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Models
{
    public class Genre
    {
        public long Id { get; set; }  // PK
        public string Name { get; set; } = null!; //not null


        //FK and References
        public ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();
    }
}
