using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Results.Base;

namespace Services.Interfaces
{
    public interface IStatService
    {
        Task<Result<int>> GetRegistrationCount(long userId, DateTime? start = null, DateTime? end = null);
        Task<Result<int>> GetReviewCount(long userId, DateTime? start = null, DateTime? end = null);
        Task<Result<int>> GetCommentCount(long userId, DateTime? start = null, DateTime? end = null);
        Task<Result<int>> GetCollectionCount(long userId, DateTime? start = null, DateTime? end = null);

        //--- hard
        Task<Result<List<(string, int)>>> GetAvarageRating(long userId, List<string>? genreExternalIds);
        Task<Result<int>> GetActiveUserCount(long userId, DateTime? start = null, DateTime? end = null);
    }
}
