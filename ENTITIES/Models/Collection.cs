using ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Models
{
    public class Collection
    {
        public ulong Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now; //not null | has default
        public string Name { get; set; } = null!;  //not null
        public bool IsPublic { get; set; } = false; //not null | has default
        public SystemCollectionType? SystemCollectionType { get; set; } = null; //nullable | has default

        //FK and References
        public ulong UserId { get; set; } //not null
        public User User { get; set; } = null!;

        public ICollection<Manga> Mangas { get; set; } = new List<Manga>();
    }
}
