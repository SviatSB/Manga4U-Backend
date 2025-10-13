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

        Task<bool> BanAsync(long userId);
        Task<bool> UnBanAsync(long userId);

        Task<bool> MuteAsync(long userId);
        Task<bool> UnMuteAsync(long userId);

        Task<bool> PromoteAsync(long userId);
        Task<bool> DemoteAsync(long userId);
    }
}
