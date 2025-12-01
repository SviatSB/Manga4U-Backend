using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Results.Base;
using Domain.Models;

namespace Services.Interfaces
{
    public interface IReviewService
    {
        Task<Result> AddReviewAsync(long userId, string mangaExternalId, int stars, string? text);
        Task<Result> DeleteReviewAsync(long userId, long reviewId);
        Task<Result<PagedResult<Review>>> GetReviewsByMangaAsync(string mangaExternalId, int skip, int take);
        Task<double> GetAverageStarsAsync(string mangaExternalId);
    }
}
