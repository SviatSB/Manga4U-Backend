using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.DTOs
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Login { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public string Nickname { get; set; } = null!;
        public bool IsMuted { get; set; }
        public DateTime? MuteExpire { get; set; }
        public string AvatarUrl { get; set; } = "avatar/default.png";
        
        // Note: PasswordHash is excluded for security reasons
    }
}
