using ENTITIES.DTOs.AccountDTOs;
using ENTITIES.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace SERVICES.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AccountService(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string login, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(login);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "user not exists" });

            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<string> GenerateTestTokenAsync(User user, IList<string> roles)
        {
            return _jwtTokenGenerator.GenerateToken(user, roles);
        }

        public async Task<string?> LoginAsync(string login, string password)
        {
            var user = await _userManager.FindByNameAsync(login);
            if (user == null)
                return null;

            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isValid)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtTokenGenerator.GenerateToken(user, roles);
        }

        public async Task<IdentityResult> RegisterAsync(string login, string password, string? nickname)
        {

            var existing = await _userManager.FindByNameAsync(login);
            if (existing != null)
                return IdentityResult.Failed(new IdentityError { Description = "such login already exists" });

            if (string.IsNullOrWhiteSpace(nickname)) nickname = login;

            var user = new User
            { 
                UserName = login,
                Nickname = nickname
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, "User");

            return result;
        }
    }
}
