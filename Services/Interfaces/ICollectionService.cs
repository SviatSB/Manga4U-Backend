using Services.Results;
using Domain.Models;

namespace Services.Interfaces
{
    public interface ICollectionService
    {
        //добавить системные коллекции
        Task<Result> AddSystemCollectionsAsync(long userId);

        //добавить мангу в коллекцию
        Task<Result> AddMangaToCollectionAsync(long userId, long collectionId, string mangaExternalId);

        //удалить мангу из коллекции
        Task<Result> RemoveMangaFromCollectionAsync(long user, long collectionId, string mangaExternalId);
        Task<Result<Collection>> CreateCollectionAsync(long userId, string name);
        Task<Result> DeleteCollectionAsync(long userId, long collectionId);

        Task<Result> RenameCollectionAsync(long userId, long collectionId, string newName);
    }
}
