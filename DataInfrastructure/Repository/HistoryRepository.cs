using System.Collections.Generic;
using System.Threading.Tasks;
using DataInfrastructure.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly MyDbContext _db;
        public HistoryRepository(MyDbContext db)
        {
            _db = db;
        }

        public Task<List<History>> GetUserHistoriesAsync(long userId)
        {
            return _db.Histories
                .Include(h => h.Manga)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.UpdatedAt)
                .ToListAsync();
        }

        public Task<History?> GetAsync(long userId, string mangaExternalId)
        {
            return _db.Histories
                .Include(h => h.Manga)
                .Where(h => h.UserId == userId && h.MangaExternalId == mangaExternalId)
                .SingleOrDefaultAsync();
        }

        public async Task AddAsync(History history)
        {
            await _db.Histories.AddAsync(history);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(History history)
        {
            _db.Histories.Update(history);
            await _db.SaveChangesAsync();
        }

        public Task<List<long>> GetLastMangaIdsAsync(long userId, int limit)
        {
            return _db.Histories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.UpdatedAt)
                .Select(h => h.MangaId)
                .Distinct()
                .Take(limit)
                .ToListAsync();
        }
    }
}
