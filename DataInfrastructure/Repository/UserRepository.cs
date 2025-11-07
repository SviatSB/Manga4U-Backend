using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using SharedConfiguration.Options;

namespace DataInfrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly MyDbContext _myDbContext;
        private readonly IAvatarStorage _avatarStorage;
        private readonly AppAzureStorageOptions _azureStorageOptions;

        public UserRepository(UserManager<User> userManager, MyDbContext myDbContext, IAvatarStorage avatarStorage, IOptions<AppAzureStorageOptions> azureOptions)
        {
            _userManager = userManager;
            _myDbContext = myDbContext;
            _avatarStorage = avatarStorage;
            _azureStorageOptions = azureOptions.Value;
        }

        public Task<User?> FindAsync(string login) => _userManager.FindByNameAsync(login);
        public Task<User?> FindAsync(long userId) => _userManager.FindByIdAsync(userId.ToString());
        public Task<IdentityResult> CreateAsync(User user, string password) => _userManager.CreateAsync(user, password);
        public Task<IdentityResult> AddToRoleAsync(User user, string role) => _userManager.AddToRoleAsync(user, role);
        public Task<bool> CheckPasswordAsync(User user, string password) => _userManager.CheckPasswordAsync(user, password);
        public Task<IList<string>> GetRolesAsync(User user) => _userManager.GetRolesAsync(user);
        public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
            => _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        //public Task<IdentityResult> UpdateAsync(User user) => _userManager.UpdateAsync(user);



        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
        public async Task ChangeNicknameAsync(User user, string newNickname)
        {
            user.Nickname = newNickname;
            await _userManager.UpdateAsync(user);
        }

        public async Task ChangeLanguageAsync(User user, string language)
        {
            if (string.IsNullOrWhiteSpace(language))
                return;

            user.Language = language.Trim().ToLowerInvariant();
            await _userManager.UpdateAsync(user);
        }

        public async Task ChangeAboutMyselfAsync(User user, string? about)
        {
            user.AboutMyself = string.IsNullOrWhiteSpace(about) ? null : about;
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> ChangeAvatarAsync(User user, IFormFile file)
        {
            //TODO: удалять старую

            if (file == null || file.Length == 0)
                return false;

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}-{user.UserName}{extension}";

            using (var stream = file.OpenReadStream())
            {
                user.AvatarUrl = await _avatarStorage.UploadAsync(stream, fileName, file.ContentType);
                var result = await _userManager.UpdateAsync(user);
            }

            return true;
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return null;

            // Якщо логін зберігається у полі UserName або Email — підлаштовуємось:
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == login || u.Email == login);
        }

        public async Task<bool> ResetAvatarAsync(User user)
        {
            if (user == null)
                return false;

            //TODO: удалять аватарки

            // Встановлюємо дефолтний
            user.AvatarUrl = _azureStorageOptions.DefaultAvatarUrl;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }


        public async Task<bool> BanAsync(User user)
        {
            user.IsBanned = true;
            user.IsMuted = true; // 🔗 логика: бан включает мьют
            var res = await _userManager.UpdateAsync(user);
            return res.Succeeded;
        }

        public async Task<bool> UnBanAsync(User user)
        {
            user.IsBanned = false;
            user.IsMuted = false;
            var res = await _userManager.UpdateAsync(user);
            return res.Succeeded;
        }

        public async Task<bool> MuteAsync(User user)
        {
            user.IsMuted = true;
            var res = await _userManager.UpdateAsync(user);
            return res.Succeeded;
        }

        public async Task<bool> UnMuteAsync(User user)
        {
            user.IsMuted = false;
            var res = await _userManager.UpdateAsync(user);
            return res.Succeeded;
        }

        public async Task<bool> PromoteAsync(User user)
        {
            var res = await _userManager.AddToRoleAsync(user, "Admin");
            return res.Succeeded ? true : false;
        }

        public async Task<bool> DemoteAsync(User user)
        {
            var res = await _userManager.RemoveFromRoleAsync(user, "Admin");
            return res.Succeeded ? true : false;
        }

        public async Task<bool> IsAdmin(User user)
        {
            var res = await _userManager.IsInRoleAsync(user, "Admin");
            return res;
        }

        public async Task<bool> IsOwner(User user)
        {
            var res = await _userManager.IsInRoleAsync(user, "Owner");
            return res;
        }

        public async Task<(IReadOnlyList<User> Users, int TotalCount)> QueryUsersAsync(
            int skip,
            int take,
            string? nickname,
            string? login,
            IList<string>? roles)
        {
            if (skip < 0) skip = 0;
            if (take <= 0) take = 20;
            if (take > 200) take = 200;

            IQueryable<User> query = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(nickname))
            {
                query = query.Where(u => u.Nickname.Contains(nickname));
            }

            if (!string.IsNullOrWhiteSpace(login))
            {
                query = query.Where(u => u.UserName!.Contains(login));
            }

            if (roles != null && roles.Count > 0)
            {
                var normalizedRoles = roles.Where(r => !string.IsNullOrWhiteSpace(r))
                                           .Select(r => r.Trim())
                                           .ToList();
                if (normalizedRoles.Count > 0)
                {
                    var usersInRoles = from ur in _myDbContext.UserRoles
                                       join r in _myDbContext.Roles on ur.RoleId equals r.Id
                                       where normalizedRoles.Contains(r.Name!)
                                       select ur.UserId;

                    query = query.Where(u => usersInRoles.Contains(u.Id));
                }
            }

            var total = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.UserName)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (users, total);
        }
    }
}
