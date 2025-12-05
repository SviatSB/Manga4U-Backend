using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInfrastructure.Interfaces
{
    public interface IStatQueryService
    {
        Task<int> GetRegistrationCountAsync(DateTime? start = null, DateTime? end = null);
        Task<int> GetReviewCountAsync(DateTime? start = null, DateTime? end = null);
        Task<int> GetCommentCountAsync(DateTime? start = null, DateTime? end = null);
        Task<int> GetCollectionCountAsync(DateTime? start = null, DateTime? end = null);

        //--- hard

        Task<List<(string, int)>> GetAvarageRatingAsync(List<string>? genreExternalIds);
        Task<int> GetActiveUserCountAsync(DateTime? start = null, DateTime? end = null);
    }
}
