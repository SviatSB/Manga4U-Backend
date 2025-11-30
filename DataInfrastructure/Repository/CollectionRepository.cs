
using DataInfrastructure.Interfaces;

using Domain.Models;

namespace DataInfrastructure.Repository
{
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
        public CollectionRepository(MyDbContext myDbContext) : base(myDbContext) { }

        public Task<IQueryable<Collection>> GetUserCollectionsAsync(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
