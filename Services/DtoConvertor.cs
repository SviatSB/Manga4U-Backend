using Domain.Models;

using Services.DTOs.CollectionDTOs;
using Services.DTOs.MangaDTOs;
using Services.DTOs.ModelsDTOs;
using Services.DTOs.ReviewDTOs;

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

        public static MangaShortDto CreateMangaShortDto(Manga manga)
        {
            return new MangaShortDto
            {
                Name = manga.Name,
                ExternalId = manga.ExternalId
            };
        }

        public static List<MangaShortDto> CreateMangaShortDto(IEnumerable<Manga> mangas)
        {
            return mangas
                .Select(CreateMangaShortDto)
                .ToList();
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
                UserId = collection.UserId,
                Mangas = CreateMangaShortDto(collection.Mangas)
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

        public static CommentDto CreateCommentDto(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                CreationTime = comment.CreationTime,
                Text = comment.Text,
                UserId = comment.UserId,
                RepliedCommentId = comment.RepliedCommentId,
                ChapterExternalId = comment.ChapterExternalId,
                UserNickname = comment.User?.Nickname,
                UserAvatarUrl = comment.User?.AvatarUrl
            };
        }

        public static List<CommentDto> CreateCommentDto(IEnumerable<Comment> comments)
        {
            return comments.Select(CreateCommentDto).ToList();
        }

        public static ReviewResponseDto CreateReviewDto(Review review)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                Stars = review.Stars,
                Text = review.Text,
                CreationTime = review.CreationTime,
                UserId = review.UserId,
                UserNickname = review.User?.Nickname,
                UserAvatarUrl = review.User?.AvatarUrl
            };
        }

        public static List<ReviewResponseDto> CreateReviewDto(IEnumerable<Review> reviews)
        {
            return reviews.Select(CreateReviewDto).ToList();
        }
    }
}
