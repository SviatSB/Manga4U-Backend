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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(MyDbContext myDbContext) : base(myDbContext) { }

        public async Task<(IReadOnlyList<Comment> Items, int TotalCount, int ReplyCount)> GetRootCommentsByChapterAsync(string chapterExternalId, int skip, int take)
        {
            // root comments are those with RepliedCommentId == null
            var query = _myDbContext.Comments
                .Where(c => c.ChapterExternalId == chapterExternalId && c.RepliedCommentId == null)
                .OrderByDescending(c => c.IsPined)
                .ThenByDescending(c => c.CreationTime);

            var total = await query.CountAsync();

            var items = await query
                .Skip(skip)
                .Take(take)
                .Include(c => c.User)
                .ToListAsync();

            // count replies for these root comments
            var rootIds = items.Select(c => c.Id).ToArray();
            int replyCount = 0;
            if (rootIds.Length > 0)
            {
                replyCount = await _myDbContext.Comments.CountAsync(c => c.RepliedCommentId != null && rootIds.Contains(c.RepliedCommentId.Value));
            }

            return (items, total, replyCount);
        }

        public async Task<(IReadOnlyList<Comment> Items, int TotalCount, int ReplyCount)> GetRepliesByCommentIdAsync(long parentCommentId, int skip, int take)
        {
            var query = _myDbContext.Comments
                .Where(c => c.RepliedCommentId == parentCommentId)
                .OrderByDescending(c => c.IsPined)
                .ThenByDescending(c => c.CreationTime);

            var total = await query.CountAsync();

            var items = await query
                .Skip(skip)
                .Take(take)
                .Include(c => c.User)
                .ToListAsync();

            // replies may have nested replies; count of replies to the parent is total
            int replyCount = total;

            return (items, total, replyCount);
        }
    }
}
