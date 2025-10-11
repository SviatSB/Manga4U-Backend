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



        //FK and References
        public int UserId { get; set; }  // FK User
        public int MangaId { get; set; }  // FK Manga
        public User User { get; set; }
        public Manga Manga { get; set; }

        //на который отвечают
        public int? RepliedCommentId { get; set; }  // FK Comment (DELETE CASCADE)
        public Comment? RepliedComment { get; set; }

        //которыми ответили на этот
        public ICollection<Comment> RepliesComments { get; set; } = new List<Comment>();


    }
}
