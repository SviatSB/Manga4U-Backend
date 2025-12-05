using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInfrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Other
{
    public class StatQueryService(MyDbContext myDbContext) : IStatQueryService
    {
        private static readonly DateTime ValidDateMin = new DateTime(2000, 1, 1);

        public async Task<int> GetActiveUserCountAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = myDbContext.Users.AsQueryable()
                .Where(u => u.LastActivity >= ValidDateMin);

            if (start.HasValue)
                query = query.Where(u => u.LastActivity >= start.Value);
            if (end.HasValue)
                query = query.Where(u => u.LastActivity <= end.Value);

            return await query.CountAsync();
        }

        public async Task<List<(string, int)>> GetAvarageRatingAsync(List<string>? genreExternalIds)
        {
            // genres are Tags with Group == "genre"
            var tagsQuery = myDbContext.Tags
                .Where(t => t.Group == "genre");

            if (genreExternalIds != null && genreExternalIds.Count > 0)
            {
                var set = genreExternalIds.Where(id => !string.IsNullOrWhiteSpace(id)).Select(id => id.Trim()).ToHashSet(StringComparer.OrdinalIgnoreCase);
                tagsQuery = tagsQuery.Where(t => set.Contains(t.TagExternalId));
            }

            var list = await tagsQuery
                .Select(t => new
                {
                    Name = t.Name,
                    Avg = t.Mangas
                        .SelectMany(m => m.Reviews)
                        .Select(r => (double?)r.Stars)
                        .Average()
                })
                .ToListAsync();

            var result = list
                .Select(x => (x.Name ?? string.Empty, (int) Math.Round(x.Avg ?? 0)))
                .ToList();

            return result;
        }

        public async Task<int> GetCollectionCountAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = myDbContext.Collections.AsQueryable()
                .Where(c => c.CreationTime >= ValidDateMin);

            if (start.HasValue)
                query = query.Where(c => c.CreationTime >= start.Value);
            if (end.HasValue)
                query = query.Where(c => c.CreationTime <= end.Value);

            return await query.CountAsync();
        }

        public async Task<int> GetCommentCountAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = myDbContext.Comments.AsQueryable()
                .Where(c => c.CreationTime >= ValidDateMin);

            if (start.HasValue)
                query = query.Where(c => c.CreationTime >= start.Value);
            if (end.HasValue)
                query = query.Where(c => c.CreationTime <= end.Value);

            return await query.CountAsync();
        }

        public async Task<int> GetRegistrationCountAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = myDbContext.Users.AsQueryable()
                .Where(u => u.RegistrationTime >= ValidDateMin);

            if (start.HasValue)
                query = query.Where(u => u.RegistrationTime >= start.Value);
            if (end.HasValue)
                query = query.Where(u => u.RegistrationTime <= end.Value);

            return await query.CountAsync();
        }

        public async Task<int> GetReviewCountAsync(DateTime? start = null, DateTime? end = null)
        {
            var query = myDbContext.Reviews.AsQueryable()
                .Where(r => r.CreationTime >= ValidDateMin);

            if (start.HasValue)
                query = query.Where(r => r.CreationTime >= start.Value);
            if (end.HasValue)
                query = query.Where(r => r.CreationTime <= end.Value);

            return await query.CountAsync();
        }
    }
}
