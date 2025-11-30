
using System;

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
            var validation = await ValidateMangaOperation(userId, collectionId, mangaExternalId);
            if (!validation.IsSucceed)
                return Result.Failure(validation.ErrorMessage);

            var (collection, manga) = validation.Value;

            if (!collection.Mangas.Any(m => m.Id == manga.Id))
                collection.Mangas.Add(manga);

            await collectionRepository.SaveChangesAsync();

            return Result.Success();
        }


        private async Task<Result<(Collection collection, Manga manga)>>
            ValidateMangaOperation(long userId, long collectionId, string mangaExternalId)
        {
            var user = await userRepository.FindWithCollections(userId);
            if (user is null)
                return Result<(Collection, Manga)>.Failure("No such user");

            var collectionResult = EnsureUserOwnsCollection(user, collectionId);
            if (!collectionResult.IsSucceed)
                return Result<(Collection, Manga)>.Failure(collectionResult.ErrorMessage);

            var collection = collectionResult.Value;

            var mangaResult = await mangaService.GetOrAdd(mangaExternalId);
            if (!mangaResult.IsSucceed || mangaResult.Value is null)
                return Result<(Collection, Manga)>.Failure("Manga not found");

            return Result<(Collection, Manga)>.Success((collection, mangaResult.Value));
        }


        private Result<Collection> EnsureUserOwnsCollection(User user, long collectionId)
        {
            var collection = user.Collections.FirstOrDefault(c => c.Id == collectionId);

            return collection is null
                ? Result<Collection>.Failure("Collection not found or not owned by user")
                : Result<Collection>.Success(collection);
        }



        public async Task<Result> RemoveMangaFromCollectionAsync(long userId, long collectionId, string mangaExternalId)
        {
            var validation = await ValidateMangaOperation(userId, collectionId, mangaExternalId);
            if (!validation.IsSucceed)
                return Result.Failure(validation.ErrorMessage);

            var (collection, manga) = validation.Value;

            var item = collection.Mangas.FirstOrDefault(m => m.Id == manga.Id);
            if (item is not null)
                collection.Mangas.Remove(item);

            await collectionRepository.SaveChangesAsync();

            return Result.Success();
        }

        //-------------------------------------------------------------

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

        public async Task<Result<Collection>> CreateCollectionAsync(long userId, string name)
        {
            var user = await userRepository.FindWithCollections(userId);
            if (user is null)
                return Result<Collection>.Failure("No such user");

            var collection = new Collection
            {
                Name = name,
            };

            user.Collections.Add(collection);
            await collectionRepository.SaveChangesAsync();

            return Result<Collection>.Success(collection);
        }

        public async Task<Result> DeleteCollectionAsync(long userId, long collectionId)
        {
            var user = await userRepository.FindWithCollections(userId);
            if (user is null)
                return Result.Failure("No such user");

            var ownership = EnsureUserOwnsCollection(user, collectionId);
            if (!ownership.IsSucceed || ownership.Value is null)
                return Result.Failure(ownership.ErrorMessage);

            var collection = ownership.Value;

            if (collection.SystemCollectionType is not null)
                return Result.Failure("Cannot delete a system collection");

            user.Collections.Remove(collection);
            await collectionRepository.SaveChangesAsync();

            return Result.Success();
        }

        // Переименовать коллекцию: только владелец и только если не системная
        public async Task<Result> RenameCollectionAsync(long userId, long collectionId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Failure("Name cannot be empty");

            var user = await userRepository.FindWithCollections(userId);
            if (user is null)
                return Result.Failure("No such user");

            var ownership = EnsureUserOwnsCollection(user, collectionId);
            if (!ownership.IsSucceed || ownership.Value is null)
                return Result.Failure(ownership.ErrorMessage);

            var collection = ownership.Value;

            if (collection.SystemCollectionType is not null)
                return Result.Failure("Cannot rename a system collection");

            collection.Name = newName;
            await collectionRepository.SaveChangesAsync();

            return Result.Success();
        }
    }
}
