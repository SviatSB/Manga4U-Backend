using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInfrastructure.Interfaces
{
    public interface IStatService
    {
        Task<int> GetRegistrationCountAsync(DateTime? start = null, DateTime? end = null);
        Task<int> GetReviewCountAsync(DateTime? start = null, DateTime? end = null);
        Task<int> GetCommentCountAsync(DateTime? start = null, DateTime? end = null);
        Task<int> GetCollectionCountAsync(DateTime? start = null, DateTime? end = null);

        //--- hard

        Task<int> GetAvarageRatingAsync(List<string>? genreExternalIds);
    }
}
