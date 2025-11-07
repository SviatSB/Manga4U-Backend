using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser<long>
    {
        //public long Id { get; set; }
        //public string Login { get; set; } = null!; //not null | unique
        //public string PasswordHash { get; set; } = null!; //not null
        //public bool IsAdmin { get; set; } = false; //not null | has default

        [MinLength(3)]
        [MaxLength(16)]
        public string Nickname { get; set; } = null!;  //not null | has default (login+random number)
        public string? AboutMyself { get; set; } = null; //nullable

        [Length(2, 2)]
        public string Language { get; set; } = "ua"; //not null | has default
        public bool IsMuted { get; set; } = false; //not null | has default
        public bool IsBanned { get; set; } = false; //not null | has default
        public string AvatarUrl { get; set; } = "https://mangastorageaccount.blob.core.windows.net/avatars/default.png"; //not null | has default


        //FK and References
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
        public ICollection<History> Histories { get; set; } = new List<History>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
