using Services.Results;

namespace Services.Interfaces
{
    public interface ICollectionService
    {
        //добавить системные коллекции
        Task<Result> AddSystemCollectionsAsync(long userId);

        //добавить мангу в коллекцию
        Task<Result> AddMangaToCollectionAsync(long userId, long collectionId, string mangaExternalId);
        //Task<Result> AddMangaToCollection(long userId, long collectionId, long mangaInnerId);

        //удалить мангу из коллекции
        Task<Result> RemoveMangaFromCollectionAsync(long user, long collectionId, string mangaExternalId);
        //Task<Result> RemoveMangaFromCollection(long user, long collectionId, long mangaInnerId);

        //проверка на принадлежность коллекции к пользователю
        Task<Result<bool>> IsUserCollectionAsync(long userId, long collectionId);
    }
}
