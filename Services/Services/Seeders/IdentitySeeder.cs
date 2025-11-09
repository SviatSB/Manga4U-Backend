using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Services.Interfaces;

using SharedConfiguration.Options;

namespace Services.Services.Seeders
{
    //TODO тут напрямую исползьуются RoleManager и UserManager, а по хорошему бы через наши сервисы это должно проходить. Но это не критично
    public class IdentitySeeder : ISeeder
    {
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly AppSeedOptions _options;

        public IdentitySeeder(
            RoleManager<IdentityRole<long>> roleManager,
            UserManager<User> userManager,
            IOptions<AppSeedOptions> options)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _options = options.Value;
        }

        public async Task SeedAsync()
        {
            if (string.IsNullOrWhiteSpace(_options.OwnerLogin) || string.IsNullOrWhiteSpace(_options.OwnerPassword))
                throw new ArgumentNullException("Seed data has null references");

            string[] roles = { "Owner", "Admin", "User" };
            foreach (var role in roles)
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole<long>(role));

            var owner = await _userManager.FindByNameAsync(_options.OwnerLogin);
            if (owner == null)
            {
                var newOwner = new User
                {
                    UserName = _options.OwnerLogin,
                    Nickname = _options.OwnerLogin,
                    EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(newOwner, _options.OwnerPassword);
                if (result.Succeeded)
                    await _userManager.AddToRolesAsync(newOwner, roles);
            }
        }
    }
}
