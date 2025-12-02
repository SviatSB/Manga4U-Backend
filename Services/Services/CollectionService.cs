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
            var user = await userRepository.FindWithCollectionsAsync(userId);
            if (user is null)
                return Result<(Collection, Manga)>.Failure("No such user");

            var collectionResult = EnsureUserOwnsCollection(user, collectionId);
            if (!collectionResult.IsSucceed)
                return Result<(Collection, Manga)>.Failure(collectionResult.ErrorMessage);

            // load collection with mangas to ensure navigation is populated
            var collectionWithMangas = await collectionRepository.GetWithMangasAsync(collectionId);
            if (collectionWithMangas is null)
                return Result<(Collection, Manga)>.Failure("Collection not found");

            // double-check ownership
            if (collectionWithMangas.UserId != userId)
                return Result<(Collection, Manga)>.Failure("Collection not found or not owned by user");

            var mangaResult = await mangaService.GetOrAdd(mangaExternalId);
            if (!mangaResult.IsSucceed || mangaResult.Value is null)
                return Result<(Collection, Manga)>.Failure("Manga not found");

            return Result<(Collection, Manga)>.Success((collectionWithMangas, mangaResult.Value));
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
            var user = await userRepository.FindWithCollectionsAsync(userId);

            foreach (var type in Enum.GetValues <SystemCollectionType>())
            {
                if (user!.Collections.Any(c => c.SystemCollectionType == type))
                    continue;

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
            var user = await userRepository.FindWithCollectionsAsync(userId);
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
            var user = await userRepository.FindWithCollectionsAsync(userId);
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

            var user = await userRepository.FindWithCollectionsAsync(userId);
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

        // Установить видимость коллекции (только владелец и только если не системная)
        public async Task<Result> SetCollectionPublicAsync(long userId, long collectionId, bool isPublic)
        {
            var user = await userRepository.FindWithCollectionsAsync(userId);
            if (user is null)
                return Result.Failure("No such user");

            var ownership = EnsureUserOwnsCollection(user, collectionId);
            if (!ownership.IsSucceed || ownership.Value is null)
                return Result.Failure(ownership.ErrorMessage);

            var collection = ownership.Value;

            if (collection.SystemCollectionType is not null)
                return Result.Failure("Cannot change visibility of a system collection");

            collection.IsPublic = isPublic;
            await collectionRepository.SaveChangesAsync();

            return Result.Success();
        }

        // Копировать публичную коллекцию для пользователя. Если коллекция не публичная — вернуть "Collection not found" без подсказки
        public async Task<Result<Collection>> CopyPublicCollectionAsync(long userId, long sourceCollectionId, string? newName = null)
        {
            var source = await collectionRepository.GetWithMangasAsync(sourceCollectionId);
            if (source == null)
            {
                // do not reveal existence
                return Result<Collection>.Failure("Collection not found");
            }

            // allow clone if public or owned by requester
            if (!source.IsPublic && source.UserId != userId)
            {
                // do not reveal existence of private collections to others
                return Result<Collection>.Failure("Collection not found");
            }

            var user = await userRepository.FindWithCollectionsAsync(userId);
            if (user is null)
                return Result<Collection>.Failure("No such user");

            // create copy
            var copy = new Collection
            {
                Name = string.IsNullOrWhiteSpace(newName) ? source.Name : newName,
                IsPublic = false,
                UserId = userId,
                SystemCollectionType = null
            };

            // copy mangas (attach existing manga references)
            foreach (var m in source.Mangas)
            {
                copy.Mangas.Add(m);
            }

            user.Collections.Add(copy);
            await collectionRepository.SaveChangesAsync();

            return Result<Collection>.Success(copy);
        }

        public async Task<Result<IEnumerable<Collection>>> GetUserSystemCollectionsAsync(long userId)
        {
            var user = await userRepository.FindAsync(userId);
            if (user is null) return Result<IEnumerable<Collection>>.Failure("No such user");

            var collections = await collectionRepository.GetUserSystemCollectionsAsync(userId);
            return Result<IEnumerable<Collection>>.Success(collections);
        }

        public async Task<Result<IEnumerable<Collection>>> GetUserNonSystemCollectionsAsync(long userId)
        {
            var user = await userRepository.FindAsync(userId);
            if (user is null) return Result<IEnumerable<Collection>>.Failure("No such user");

            var collections = await collectionRepository.GetUserNonSystemCollectionsAsync(userId);
            return Result<IEnumerable<Collection>>.Success(collections);
        }

        public async Task<Result<IEnumerable<Collection>>> SearchPublicCollectionsByNameAsync(string name)
        {
            var collections = await collectionRepository.SearchPublicCollectionsByNameAsync(name);
            return Result<IEnumerable<Collection>>.Success(collections);
        }

        public async Task<Result<Collection>> GetCollectionWithContentAsync(long? userId, long collectionId)
        {
            var collection = await collectionRepository.GetByIdWithContentAsync(collectionId);
            if (collection == null)
                return Result<Collection>.Failure("Collection not found");

            // allow access if public
            if (collection.IsPublic)
                return Result<Collection>.Success(collection);

            // otherwise check owner
            if (collection.UserId == userId)
                return Result<Collection>.Success(collection);

            return Result<Collection>.Failure("Collection not found");
        }
    }
}
