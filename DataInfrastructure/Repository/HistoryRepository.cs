using System.Collections.Generic;
using System.Threading.Tasks;
using DataInfrastructure.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class HistoryRepository : Repository<History>, IHistoryRepository
    {
        public HistoryRepository(MyDbContext myDbContext) : base(myDbContext) { }

        public Task<List<History>> GetUserHistoriesAsync(long userId)
        {
            return _myDbContext.Histories
                .Include(h => h.Manga)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.UpdatedAt)
                .ToListAsync();
        }

        public Task<History?> GetAsync(long userId, string mangaExternalId)
        {
            return _myDbContext.Histories
                .Include(h => h.Manga)
                .Where(h => h.UserId == userId && h.MangaExternalId == mangaExternalId)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(History history)
        {
            _myDbContext.Histories.Update(history);
            await _myDbContext.SaveChangesAsync();
        }

        public Task<List<long>> GetLastMangaIdsAsync(long userId, int limit)
        {
            return _myDbContext.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.UpdatedAt)
                .Select(h => h.MangaId)
                .Distinct()
                .Take(limit)
                .ToListAsync();
        }
    }
}
