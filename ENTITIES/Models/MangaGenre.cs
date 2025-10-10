using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Models
{
    public class MangaGenre
    {
        public int Id { get; set; }  // PK
        public int Order { get; set; } //not null


        //FK and References
        public int MangaId { get; set; } //not null
        public int GenreId { get; set; } //not null

        public Manga Manga { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
    }
}
