using ENTITIES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DATAINFRASTRUCTURE
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services, string ownerLogin, string ownerPassword)
        {
            if (ownerLogin == null || ownerPassword == null) throw new ArgumentNullException("seed data has null references");

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<long>>>();
            var userManager = services.GetRequiredService<UserManager<User>>();

            string[] roles = { "Owner", "Admin", "User" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<long>(role));

            var owner = await userManager.FindByNameAsync(ownerLogin);
            if (owner == null)
            {
                var newOwner = new User
                {
                    UserName = ownerLogin,
                    Nickname = ownerLogin,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newOwner, ownerPassword);
                if (result.Succeeded)
                    await userManager.AddToRolesAsync(newOwner, ["Owner", "Admin", "User"]);

            }
        }
    }
}
