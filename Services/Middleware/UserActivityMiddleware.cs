using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.AspNetCore.Http;

namespace Services.Middleware
{
    public class UserActivityMiddleware
    {
        private readonly RequestDelegate _next;

        public UserActivityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepository userRepository)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                await _next(context);
                return;
            }

            var user = await userRepository.FindAsync(context.User.Identity?.Name!);
            if (user != null)
            {
                await userRepository.UpdateActivity(user);
            }

            await _next(context);
        }
    }
}
