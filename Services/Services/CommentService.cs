using DataInfrastructure.Interfaces;

using Services.Interfaces;
using Services.Results.Base;
using Services.Results.Custom;

using Domain.Models;

namespace Services.Services
{
    public class CommentService(ICommentRepository commentRepository, IUserRepository userRepository) : ICommentService
    {
        public async Task<Result> CreateCommentAsync(long userId, string mangaChapterExternalId, string text, long? parentCommentId = null)
        {
            var user = await userRepository.FindAsync(userId);

            if (user is null)
            {
                return Result.Failure("User not found.");
            }

            if (user.IsMuted)
            {
                return Result.Failure("User is muted.");
            }

            await commentRepository.AddAsync(
                new Comment
                {
                    UserId = userId,
                    ChapterExternalId = mangaChapterExternalId,
                    Text = text,
                    RepliedCommentId = parentCommentId,
                }
            );

            return Result.Success();
        }

        public async Task<Result> DeleteCommentAsync(long userId, long commentId)
        {
            var user = await userRepository.FindAsync(userId);

            if (user is null)
            {
                return Result.Failure("User not found.");
            }

            var comment = await commentRepository.GetAsync(commentId);

            if (comment is null)
            {
                return Result.Failure("Comment not found.");
            }

            //----------------

            var roles = await userRepository.GetRolesAsync(user);

            if (!roles.Contains("Admin") && comment.UserId != userId)
            {
                return Result.Failure("You do not have permission to delete this comment.");
            }

            await commentRepository.DeleteAsync(comment.Id);
            return Result.Success();
        }

        public async Task<Result<CommentPagedResult>> GetCommentRepliesAsync(int take, int skip, long commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<CommentPagedResult>> GetRootCommentsAsync(int take, int skip, string mangaChapterExternalId)
        {
            throw new NotImplementedException();
        }
    }
}
