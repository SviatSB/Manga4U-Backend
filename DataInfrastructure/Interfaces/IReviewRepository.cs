using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataInfrastructure.Repository;

using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<(IReadOnlyList<Review> Items, int TotalCount)> GetReviewsByMangaIdAsync(long mangaId, int skip, int take);
        Task<bool> UserHasReviewAsync(long userId, long mangaId);
        Task<double> GetAverageStarsAsync(long mangaId);
    }
}
