
using Azure.Core;

using DataInfrastructure.Interfaces;

using Domain.Models;

using Newtonsoft.Json.Linq;

using Services.Interfaces;
using Services.Results;

namespace Services.Services
{
    public class CollectionService(IUserRepository userRepository, IMangaService mangaService, ICollectionRepository collectionRepository) : ICollectionService
    {
        public async Task<Result> AddMangaToCollectionAsync(long userId, long collectionId, string mangaExternalId)
        {
            var user = await userRepository.FindWithCollections(userId);
            if (user is null)
            {
                return Result.Failure("User not found");
            }

            var mangaResult = await mangaService.GetOrAdd(mangaExternalId);
            if (!mangaResult.IsSucceed || mangaResult.Value is null)
            {
                return Result.Failure("Manga not found");
            }

            var collection = user.Collections.FirstOrDefault(c => c.Id == collectionId);
            if (collection is null)
            {
                return Result.Failure("Collection not found or not belong to this user");
            }

            await LinkManga(mangaResult.Value, collection);

            return Result.Success();
        }

        private Task LinkManga(Manga manga, Collection collection)
        {
            collection.Mangas.Add(manga);
            return collectionRepository.SaveChangesAsync();
        }

        public async Task<Result> AddSystemCollectionsAsync(long userId)
        {

            foreach (var type in Enum.GetValues <SystemCollectionType>())
            {
                var collection = new Collection
                {
                    Name = type.ToString(),
                    SystemCollectionType = type,
                    UserId = userId
                };
                await collectionRepository.AddAsync(collection);
            }

            return Result.Success();
        }

        public async Task<Result<bool>> IsUserCollectionAsync(long userId, long collectionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> RemoveMangaFromCollectionAsync(long user, long collectionId, string mangaExternalId)
        {
            throw new NotImplementedException();
        }
    }
}
