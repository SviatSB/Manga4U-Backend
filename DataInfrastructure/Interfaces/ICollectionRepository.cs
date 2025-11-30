using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        //получить коллекции ползьователя
        Task<IEnumerable<Collection>> GetUserCollectionsAsync(long userId);
        // получить коллекцию с включёнными мангами
        Task<Collection?> GetWithMangasAsync(long id);
        // получить только системные коллекции пользователя
        Task<IEnumerable<Collection>> GetUserSystemCollectionsAsync(long userId);
        // получить только не системные коллекции пользователя
        Task<IEnumerable<Collection>> GetUserNonSystemCollectionsAsync(long userId);
        // поиск публичных коллекций по имени
        Task<IEnumerable<Collection>> SearchPublicCollectionsByNameAsync(string name);
        // Получить коллекцию с контентом независимо от владельца
        Task<Collection?> GetByIdWithContentAsync(long collectionId);
    }
}
