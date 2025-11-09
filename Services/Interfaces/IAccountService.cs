using Domain.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Services.DTOs.ModelsDTOs;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(string login, string password, string nickname);
        Task<string?> LoginAsync(string login, string password);
        Task<IdentityResult> ChangePasswordAsync(string login, string oldPassword, string newPassword);
        Task<bool> ChangeNicknameAsync(string login, string newNickname);
        Task<bool> ChangeAvatarAsync(string login, IFormFile file);
        Task<UserDto?> GetUserDtoAsync(string login);

        Task<bool> BanAsync(string actorLogin, long targetUserId);
        Task<bool> UnBanAsync(string actorLogin, long targetUserId);

        Task<bool> MuteAsync(string actorLogin, long targetUserId);
        Task<bool> UnMuteAsync(string actorLogin, long targetUserId);

        Task<bool> PromoteAsync(string actorLogin, long targetUserId);
        Task<bool> DemoteAsync(string actorLogin, long targetUserId);

        Task<PagedResult<UserDto>> GetUsersAsync(int skip, int take, string? nickname, string? login, IList<string>? roles);

        // ✅ Новий метод для повернення дефолтного аватара
        Task<bool> ResetAvatarAsync(string login);

        // set user preferences
        Task<bool> SetLanguageAsync(string login, string language);
        Task<bool> SetAboutMyselfAsync(string login, string? about);
    }
}
