using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Models
{
    public class History
    {
        public ulong Id { get; set; }
        public int LastChapter { get; set; } //not null | has default = 0?
        public int LastPage { get; set; } //not null | has default = 0?
        public DateTime UpdatedAt { get; set; } = DateTime.Now; //not null | has default


        //FK and References
        public int UserId { get; set; }  //not null
        public int MangaId { get; set; }  //not null

        public User User { get; set; } = null!;
        public Manga Manga { get; set; } = null!;
    }
}
