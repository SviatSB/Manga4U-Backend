using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc;

using Services.DTOs.AccountDTOs;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class MyController(IUserService userService) : ControllerBase
    {
        protected string? ContextLogin { get => User.Identity?.Name; }
        protected async Task<UserDto?> GetCurrentUserAsync()
        {
            Console.WriteLine("ContextLogin " + ContextLogin);
            return ContextLogin is null ? null : await userService.FindAsync(ContextLogin);
        }

    }
}
