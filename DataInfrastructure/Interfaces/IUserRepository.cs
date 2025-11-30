using Domain.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DataInfrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> FindAsync(string login);
        Task<User?> FindAsync(long userId);

        //TODO избавится от дублирования метода FindAsync
        Task<User?> GetByLoginAsync(string login);

        Task<User?> FindWithCollections(long userId);

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task ChangeNicknameAsync(User user, string newNickname);
        Task ChangeLanguageAsync(User user, string language);
        Task ChangeAboutMyselfAsync(User user, string? about);
        Task<bool> ChangeAvatarAsync(User user, IFormFile file);
        Task<bool> ResetAvatarAsync(User user);

        Task<bool> BanAsync(User user);
        Task<bool> UnBanAsync(User user);
        Task<bool> MuteAsync(User user);
        Task<bool> UnMuteAsync(User user);

        Task<bool> PromoteAsync(User user);
        Task<bool> DemoteAsync(User user);
        Task<bool> IsAdmin(User user);
        Task<bool> IsOwner(User user);

        Task<(IReadOnlyList<User> Users, int TotalCount)> QueryUsersAsync(
            int skip,
            int take,
            string? nickname,
            string? login,
            IList<string>? roles);
    }
}
