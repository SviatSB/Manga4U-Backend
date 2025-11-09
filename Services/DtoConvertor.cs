using Domain.Models;

using Services.DTOs.ModelsDTOs;

namespace Services
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
                Language = user.Language,
                AboutMyself = user.AboutMyself,
                IsMuted = user.IsMuted,
                IsBanned = user.IsBanned,
                AvatarUrl = user.AvatarUrl,
                Roles = roles
            };
        }
    }
}
