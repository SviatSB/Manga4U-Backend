using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Services.Results.Base;
using Services.Results.Custom;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        Task<Result> CreateCommentAsync(long userId, string mangaChapterExternalId, string text, long? parentCommentId = null);
        Task<Result> DeleteCommentAsync(long userId, long commentId);
        Task<Result<CommentPagedResult>> GetRootCommentsAsync(int take, int skip, string mangaChapterExternalId);
        Task<Result<CommentPagedResult>> GetCommentRepliesAsync(int take, int skip, long commentId);
        Task<Result> SetPinnedStatusAsync(long userId, long commentId, bool isPinned);
    }
}
