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
            var user = await _userRepository.FindByNameAsync(login);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "user not exists" });

            return await _userRepository.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<string?> LoginAsync(string login, string password)
        {
            var user = await _userRepository.FindByNameAsync(login);
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

            var existing = await _userRepository.FindByNameAsync(login);
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
            var user = await _userRepository.FindByNameAsync(login);
            if (user == null)
                return false;

            await _userRepository.ChangeNicknameAsync(user, newNickname);

            return true;
        }

        public async Task<bool> ChangeAvatarAsync(string login, IFormFile file)
        {
            var user = await _userRepository.FindByNameAsync(login);
            if (user == null)
                return false;

            var res = await _userRepository.ChangeAvatarAsync(user, file);

            return res;
        }

        public async Task<UserDto?> GetUserDtoAsync(string login)
        {
            var user = await _userRepository.FindByNameAsync(login);
            if (user == null)
                return null;

            var roles = await _userRepository.GetRolesAsync(user);

            return DtoConvertor.UserToDto(user, roles);
        }

        public async Task<bool> BanAsync(long userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var res = await _userRepository.BanAsync(user);
            return res;
        }

        public async Task<bool> UnBanAsync(long userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var res = await _userRepository.UnBanAsync(user);
            return res;
        }

        public async Task<bool> MuteAsync(long userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var res = await _userRepository.MuteAsync(user);
            return res;
        }

        public async Task<bool> UnMuteAsync(long userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var res = await _userRepository.UnMuteAsync(user);
            return res;
        }

        public async Task<bool> PromoteAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DemoteAsync(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
