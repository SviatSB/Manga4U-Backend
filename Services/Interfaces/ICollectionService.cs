using Domain.Models;

using Services.Results.Base;

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

        // Переименовать коллекцию (только владельцем и только если не системная)
        Task<Result> RenameCollectionAsync(long userId, long collectionId, string newName);

        // Установить видимость коллекции (только владелец и только если не системная)
        Task<Result> SetCollectionPublicAsync(long userId, long collectionId, bool isPublic);

        // Копировать публичную коллекцию для пользователя. Если коллекция не публичная — возвращаем "Collection not found" чтобы не выдать существование
        Task<Result<Collection>> CopyPublicCollectionAsync(long userId, long sourceCollectionId, string? newName = null);

        // Получить системные коллекции пользователя (сначала проверка существования пользователя)
        Task<Result<IEnumerable<Collection>>> GetUserSystemCollectionsAsync(long userId);

        // Получить не системные коллекции пользователя (сначала проверка существования пользователя)
        Task<Result<IEnumerable<Collection>>> GetUserNonSystemCollectionsAsync(long userId);

        // Поиск публичных коллекций по названию
        Task<Result<IEnumerable<Collection>>> SearchPublicCollectionsByNameAsync(string name);

        // Получить коллекцию с её содержимым если она публична или принадлежит пользователю
        Task<Result<Collection>> GetCollectionWithContentAsync(long? userId, long collectionId);
    }
}
