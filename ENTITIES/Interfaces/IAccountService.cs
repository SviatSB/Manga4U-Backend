using ENTITIES.DTOs;
using ENTITIES.DTOs.AccountDTOs;
using ENTITIES.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.Interfaces
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
    }
}
