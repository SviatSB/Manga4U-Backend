using Domain.Models;

using Services.DTOs.CollectionDTOs;
using Services.DTOs.ModelsDTOs;

namespace Services
{
    public static class DtoConvertor
    {
        public static UserDto CreateUserDto(User user, IList<string> roles)
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

        public static CollectionShortDto CreateCollectionShortDto(Collection collection)
        {
            return new CollectionShortDto
            {
                Id = collection.Id,
                Name = collection.Name,
                IsPublic = collection.IsPublic,
                CreationTime = collection.CreationTime,
                SystemCollectionType = collection.SystemCollectionType
            };
        }

        public static CollectionFullDto CreateCollectionFullDto(Collection collection)
        {
            return new CollectionFullDto
            {
                Id = collection.Id,
                Name = collection.Name,
                IsPublic = collection.IsPublic,
                CreationTime = collection.CreationTime,
                SystemCollectionType = collection.SystemCollectionType,
            };
        }

        public static List<CollectionShortDto> CreateCollectionShortDto(IEnumerable<Collection> collections)
        {
            return collections
                .Select(CreateCollectionShortDto)
                .ToList();
        }

        public static List<CollectionFullDto> CreateCollectionFullDto(IEnumerable<Collection> collections)
        {
            return collections
                .Select(CreateCollectionFullDto)
                .ToList();
        }

    }
}
