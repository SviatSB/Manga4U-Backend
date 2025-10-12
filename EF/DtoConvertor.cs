using ENTITIES.DTOs;
using ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DATAINFRASTRUCTURE
{
    public static class DtoConvertor
    {
        public static UserDto UserToDto(User user, IList<string> roles)
        {
            return new UserDto
            {
                Id = user.Id,
                Login = user.UserName!, //вроде бы встроенная проверка на наличие UserName при создании есть но тут возвращает string?
                Nickname = user.Nickname,
                IsMuted = user.IsMuted,
                IsBanned = user.IsBanned,
                AvatarUrl = user.AvatarUrl,
                Roles = roles
            };
        }
    }
}
