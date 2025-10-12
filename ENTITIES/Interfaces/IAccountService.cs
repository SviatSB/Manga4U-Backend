using ENTITIES.DTOs.AccountDTOs;
using ENTITIES.Models;
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
        Task<string> GenerateTestTokenAsync(User user, IList<string> roles);
    }
}
