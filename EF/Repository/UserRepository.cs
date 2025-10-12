using ENTITIES.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DATAINFRASTRUCTURE.Repository
{
    public class UserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly MyDbContext _myDbContext;

        public UserRepository(UserManager<User> userManager, MyDbContext myDbContext)
        {
            _userManager = userManager;
            _myDbContext = myDbContext;
        }

        public Task<User?> FindByNameAsync(string login) => _userManager.FindByNameAsync(login);
        public Task<IdentityResult> CreateAsync(User user, string password) => _userManager.CreateAsync(user, password);
        public Task<IdentityResult> AddToRoleAsync(User user, string role) => _userManager.AddToRoleAsync(user, role);
        public Task<bool> CheckPasswordAsync(User user, string password) => _userManager.CheckPasswordAsync(user, password);
        public Task<IList<string>> GetRolesAsync(User user) => _userManager.GetRolesAsync(user);
        public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
            => _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        public Task<IdentityResult> UpdateAsync(User user) => _userManager.UpdateAsync(user);

        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        public async Task ChangeNicknameAsync(User user, string newNickname)
        {
            user.Nickname = newNickname;
            await _userManager.UpdateAsync(user);
        }

        public async Task ChangeAvatarAsync(User user, IFormFile file)
        {
            var oldAvatarPath = user.AvatarUrl;

            //TODO перенесити этот путь в конфиг
            var avatarsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatar");
            Directory.CreateDirectory(avatarsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(avatarsPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            user.AvatarUrl = $"/avatar/{fileName}";
            await _userManager.UpdateAsync(user);

            File.Delete(oldAvatarPath);
        }
    }
}
