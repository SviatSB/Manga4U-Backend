using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Models
{
    public class Comment
    {
        public ulong Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now; //not null | has default
        public string Text { get; set; } = null!; //not null
        public int UserId { get; set; }  // FK User
        public int MangaId { get; set; }  // FK Manga

        //FK and References


        //RepliedCommentId

        //public int? RepliedCommentId { get; set; }  // FK Comment (DELETE CASCADE)
        //public int UserId { get; set; }  // FK User
        //public int MangaId { get; set; }  // FK Manga

        // public Comment? RepliedComment { get; set; }
        // public User User { get; set; }
        // public Manga Manga { get; set; }
    }
}
