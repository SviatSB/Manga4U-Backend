using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInfrastructure.Interfaces;

using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(MyDbContext myDbContext) : base(myDbContext) { }

        public async Task<(IReadOnlyList<Review> Items, int TotalCount)> GetReviewsByMangaIdAsync(long mangaId, int skip, int take)
        {
            var query = _myDbContext.Reviews
                .Where(r => r.MangaId == mangaId)
                .OrderByDescending(r => r.IsPined)
                .ThenByDescending(r => r.CreationTime);

            var total = await query.CountAsync();
            var items = await query.Skip(skip).Take(take).Include(r => r.User).ToListAsync();

            return (items, total);
        }

        public Task<bool> UserHasReviewAsync(long userId, long mangaId)
        {
            return _myDbContext.Reviews.AnyAsync(r => r.UserId == userId && r.MangaId == mangaId);
        }

        public async Task<double> GetAverageStarsAsync(long mangaId)
        {
            var ratings = await _myDbContext.Reviews
                .Where(r => r.MangaId == mangaId)
                .Select(r => (double)r.Stars)
                .ToListAsync();

            if (ratings.Count == 0) return 0.0;
            return ratings.Average();
        }
    }
}
