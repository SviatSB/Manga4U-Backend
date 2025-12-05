using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Results.Base;

using DataInfrastructure.Interfaces;

using Services.Interfaces;

namespace Services.Services
{
    public class StatService(IStatQueryService statQueryService, IUserRepository userRepository) : IStatService
    {
        private async Task<Result<T>> ExecuteIfAdminAsync<T>(Func<Task<T>> func, long userId)
        {
            var user = await userRepository.FindAsync(userId);

            if (user is null)
            {
                return Result<T>.Failure("No such user");
            }

            bool isAdmin = await userRepository.IsAdmin(user);

            if (isAdmin)
            {
                var result = await func();
                return Result<T>.Success(result);
            }

            return Result<T>.Failure("User does not have permission");
        }

        public async Task<Result<int>> GetActiveUserCount(long userId, DateTime? start = null, DateTime? end = null)
        {
            return await ExecuteIfAdminAsync<int>(() => statQueryService.GetActiveUserCountAsync(start, end), userId);
        }

        public async Task<Result<int>> GetRegistrationCount(long userId, DateTime? start = null, DateTime? end = null)
        {
            return await ExecuteIfAdminAsync<int>(() => statQueryService.GetRegistrationCountAsync(start, end), userId);
        }

        public async Task<Result<int>> GetReviewCount(long userId, DateTime? start = null, DateTime? end = null)
        {
            return await ExecuteIfAdminAsync<int>(() => statQueryService.GetReviewCountAsync(start, end), userId);
        }

        public async Task<Result<int>> GetCommentCount(long userId, DateTime? start = null, DateTime? end = null)
        {
            return await ExecuteIfAdminAsync<int>(() => statQueryService.GetCommentCountAsync(start, end), userId);
        }

        public async Task<Result<int>> GetCollectionCount(long userId, DateTime? start = null, DateTime? end = null)
        {
            return await ExecuteIfAdminAsync<int>(() => statQueryService.GetCollectionCountAsync(start, end), userId);
        }

        public async Task<Result<List<(string, int)>>> GetAvarageRating(long userId, List<string>? genreExternalIds)
        {
            return await ExecuteIfAdminAsync<List<(string, int)>>(() => statQueryService.GetAvarageRatingAsync(genreExternalIds), userId);
        }
    }
}
