using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<(IReadOnlyList<Comment> Items, int TotalCount, Dictionary<long,int> ReplyCounts)> GetRootCommentsByChapterAsync(string chapterExternalId, int skip, int take);
        Task<(IReadOnlyList<Comment> Items, int TotalCount, int ReplyCount)> GetRepliesByCommentIdAsync(long parentCommentId, int skip, int take);
        Task<List<long>> GetAllDescendantIdsAsync(long rootCommentId);
        Task DeleteByIdsAsync(IEnumerable<long> ids);
    }
}
