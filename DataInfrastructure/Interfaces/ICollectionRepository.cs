using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        //получить коллекции ползьователя
        Task<IQueryable<Collection>> GetUserCollectionsAsync(long userId);
    }
}
