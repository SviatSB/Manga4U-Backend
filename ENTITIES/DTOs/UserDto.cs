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
        public string Nickname { get; set; } = null!;
        public bool IsMuted { get; set; }
        public bool IsBanned { get; set; }
        public string AvatarUrl { get; set; } = null!;
        public IList<string> Roles { get; set; } = new List<string>();
        public string? AboutMyself { get; set; }
        public string Language { get; set; } = null!;

        // Note: PasswordHash is excluded for security reasons
    }
}
