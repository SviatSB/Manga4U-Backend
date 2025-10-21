using DATAINFRASTRUCTURE;
using DATAINFRASTRUCTURE.Repository;
using ENTITIES.DTOs;
using ENTITIES.DTOs.AccountDTOs;
using ENTITIES.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace SERVICES.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AccountService(UserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string login, string oldPassword, string newPassword)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "user not exists" });

            return await _userRepository.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<string?> LoginAsync(string login, string password)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null)
                return null;

            var isValid = await _userRepository.CheckPasswordAsync(user, password);
            if (!isValid)
                return null;

            var roles = await _userRepository.GetRolesAsync(user);
            return _jwtTokenGenerator.GenerateToken(user, roles);
        }

        public async Task<IdentityResult> RegisterAsync(string login, string password, string? nickname)
        {

            var existing = await _userRepository.FindAsync(login);
            if (existing != null)
                return IdentityResult.Failed(new IdentityError { Description = "such login already exists" });

            if (string.IsNullOrWhiteSpace(nickname)) nickname = login;

            var user = new User
            { 
                UserName = login,
                Nickname = nickname
            };

            var result = await _userRepository.CreateAsync(user, password);
            if (result.Succeeded)
                await _userRepository.AddToRoleAsync(user, "User");

            return result;
        }

        public async Task<bool> ChangeNicknameAsync(string login, string newNickname)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null)
                return false;

            await _userRepository.ChangeNicknameAsync(user, newNickname);

            return true;
        }

        public async Task<bool> ChangeAvatarAsync(string login, IFormFile file)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null)
                return false;

            var res = await _userRepository.ChangeAvatarAsync(user, file);

            return res;
        }

        public async Task<bool> ResetAvatarAsync(string login)
        {
            var user = await _userRepository.GetByLoginAsync(login);
            if (user == null)
                return false;

            return await _userRepository.ResetAvatarAsync(user);
        }

        public async Task<bool> SetLanguageAsync(string login, string language)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null)
                return false;

            await _userRepository.ChangeLanguageAsync(user, language);
            return true;
        }

        public async Task<bool> SetAboutMyselfAsync(string login, string? about)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null)
                return false;

            await _userRepository.ChangeAboutMyselfAsync(user, about);
            return true;
        }


        public async Task<UserDto?> GetUserDtoAsync(string login)
        {
            var user = await _userRepository.FindAsync(login);
            if (user == null)
                return null;

            var roles = await _userRepository.GetRolesAsync(user);

            return DtoConvertor.UserToDto(user, roles);
        }

        public async Task<PagedResult<UserDto>> GetUsersAsync(int skip, int take, string? nickname, string? login, IList<string>? roles)
        {
            var (users, total) = await _userRepository.QueryUsersAsync(skip, take, nickname, login, roles);

            var items = new List<UserDto>(users.Count);
            foreach (var u in users)
            {
                var uRoles = await _userRepository.GetRolesAsync(u);
                items.Add(DtoConvertor.UserToDto(u, uRoles));
            }

            return new PagedResult<UserDto>
            {
                TotalCount = total,
                Items = items
            };
        }

        public async Task<bool> BanAsync(string actorLogin, long targetUserId)
        {
            var pair = await GetActorAndTargetOrDefault(actorLogin, targetUserId);
            if (pair is null)
                return false;
            var (actorUser, targetUser) = pair.Value;

            if (!await IsHigherRole(actorUser, targetUser))
                return false;

            return await _userRepository.BanAsync(targetUser);
        }

        public async Task<bool> UnBanAsync(string actorLogin, long targetUserId)
        {
            var pair = await GetActorAndTargetOrDefault(actorLogin, targetUserId);
            if (pair is null)
                return false;
            var (actorUser, targetUser) = pair.Value;

            if (!await IsHigherRole(actorUser, targetUser))
                return false;

            return await _userRepository.UnBanAsync(targetUser);
        }

        public async Task<bool> MuteAsync(string actorLogin, long targetUserId)
        {
            var pair = await GetActorAndTargetOrDefault(actorLogin, targetUserId);
            if (pair is null)
                return false;
            var (actorUser, targetUser) = pair.Value;

            if (!await IsHigherRole(actorUser, targetUser))
                return false;

            return await _userRepository.MuteAsync(targetUser);
        }

        public async Task<bool> UnMuteAsync(string actorLogin, long targetUserId)
        {
            var pair = await GetActorAndTargetOrDefault(actorLogin, targetUserId);
            if (pair is null)
                return false;
            var (actorUser, targetUser) = pair.Value;

            if (!await IsHigherRole(actorUser, targetUser))
                return false;

            return await _userRepository.UnMuteAsync(targetUser);
        }

        public async Task<bool> PromoteAsync(string actorLogin, long targetUserId)
        {
            var pair = await GetActorAndTargetOrDefault(actorLogin, targetUserId);
            if (pair is null)
                return false;
            var (actorUser, targetUser) = pair.Value;

            if (await _userRepository.IsOwner(actorUser))
                return await _userRepository.PromoteAsync(targetUser);

            return false;
        }

        public async Task<bool> DemoteAsync(string actorLogin, long targetUserId)
        {
            var pair = await GetActorAndTargetOrDefault(actorLogin, targetUserId);
            if (pair is null)
                return false;
            var (actorUser, targetUser) = pair.Value;

            if (await _userRepository.IsOwner(actorUser))
                return await _userRepository.DemoteAsync(targetUser);

            return false;

        }

        public async Task<bool> IsHigherRole(User actor, User target)
        {
            if (await _userRepository.IsOwner(actor)) return true;
            if (await _userRepository.IsOwner(target)) return false;

            bool actorIsAdmin = await _userRepository.IsAdmin(actor);
            bool targetIsAdmin = await _userRepository.IsAdmin(target);

            return actorIsAdmin && !targetIsAdmin;
        }

        private async Task<(User actor, User target)?> GetActorAndTargetOrDefault(string actorLogin, long targetUserId)
        {
            var targetUser = await _userRepository.FindAsync(targetUserId);
            if (targetUser is null)
                return null;

            var actorUser = await _userRepository.FindAsync(actorLogin);
            if (actorUser is null)
                return null;

            return (actorUser, targetUser);
        }
    }
}
