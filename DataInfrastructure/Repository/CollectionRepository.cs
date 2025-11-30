using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
        public CollectionRepository(MyDbContext myDbContext) : base(myDbContext) { }

        public Task<IEnumerable<Collection>> GetUserCollectionsAsync(long userId)
        {
            var list = _myDbContext.Collections.Where(c => c.UserId == userId).AsEnumerable();
            return Task.FromResult(list);
        }

        public async Task<Collection?> GetWithMangasAsync(long id)
        {
            return await _myDbContext.Collections
                .Include(c => c.Mangas)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public Task<IEnumerable<Collection>> GetUserSystemCollectionsAsync(long userId)
        {
            var list = _myDbContext.Collections
                .Where(c => c.UserId == userId && c.SystemCollectionType != null)
                .AsEnumerable();
            return Task.FromResult(list);
        }

        public Task<IEnumerable<Collection>> GetUserNonSystemCollectionsAsync(long userId)
        {
            var list = _myDbContext.Collections
                .Where(c => c.UserId == userId && c.SystemCollectionType == null)
                .AsEnumerable();
            return Task.FromResult(list);
        }

        public Task<IEnumerable<Collection>> SearchPublicCollectionsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Task.FromResult(Enumerable.Empty<Collection>());

            var lowered = name.Trim().ToLowerInvariant();
            var list = _myDbContext.Collections
                .Where(c => c.IsPublic && EF.Functions.Like(c.Name.ToLower(), $"%{lowered}%"))
                .AsEnumerable();

            return Task.FromResult(list);
        }

        public async Task<Collection?> GetByIdWithContentAsync(long collectionId)
        {
            return await _myDbContext.Collections
                .Include(c => c.Mangas)
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == collectionId);
        }
    }
}
