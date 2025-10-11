using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Models
{
    public class User : IdentityUser<long>
    {
        //public long Id { get; set; }
        //public string Login { get; set; } = null!; //not null | unique
        //public string PasswordHash { get; set; } = null!; //not null
        //public bool IsAdmin { get; set; } = false; //not null | has default
        public string Nickname { get; set; } = null!;  //not null | has default (login+random number)
        public bool IsMuted { get; set; } = false; //not null | has default
        public DateTime? MuteExpire { get; set; } = null; //nullable | has default = null
        public string AvatarUrl { get; set; } = "avatar/default.png"; //not null | has default


        //FK and References
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
        public ICollection<History> Histories { get; set; } = new List<History>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
