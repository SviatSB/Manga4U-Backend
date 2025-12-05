using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataInfrastructure.Interfaces;

namespace DataInfrastructure.Other
{
    public class StatService : IStatService
    {
        public Task<int> GetAvarageRatingAsync(List<string>? genreExternalIds)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCollectionCountAsync(DateTime? start = null, DateTime? end = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCommentCountAsync(DateTime? start = null, DateTime? end = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetRegistrationCountAsync(DateTime? start = null, DateTime? end = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetReviewCountAsync(DateTime? start = null, DateTime? end = null)
        {
            throw new NotImplementedException();
        }
    }
}
