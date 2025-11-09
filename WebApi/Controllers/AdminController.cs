using Domain.Results;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.DTOs.ModelsDTOs;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController(IAccountService accountService, IUserService userService) : MyController(userService)
    {
        private readonly IAccountService _accountService = accountService;

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/ban")]
        public async Task<IActionResult> Ban([FromRoute] long userId)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            var res = await _accountService.BanAsync(this.ContextLogin, userId);
            return res ? Ok("User has been banned.") : BadRequest("Something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/unban")]
        public async Task<IActionResult> UnBan([FromRoute] long userId)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            var res = await _accountService.UnBanAsync(this.ContextLogin, userId);
            return res ? Ok("User has been unbanned.") : BadRequest("Something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/mute")]
        public async Task<IActionResult> Mute([FromRoute] long userId)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            var res = await _accountService.MuteAsync(this.ContextLogin, userId);
            return res ? Ok("User has been muted.") : BadRequest("Something went wrong");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/unmute")]
        public async Task<IActionResult> UnMute([FromRoute] long userId)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            var res = await _accountService.UnMuteAsync(this.ContextLogin, userId);
            return res ? Ok("User has been unmuted.") : BadRequest("Something went wrong");
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("user/{userId}/promote")]
        public async Task<IActionResult> Promote([FromRoute] long userId)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            var res = await _accountService.PromoteAsync(this.ContextLogin, userId);
            return res ? Ok("User has been promoted.") : BadRequest("Something went wrong");
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("user/{userId}/demote")]
        public async Task<IActionResult> Demote([FromRoute] long userId)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            var res = await _accountService.DemoteAsync(this.ContextLogin, userId);
            return res ? Ok("User has been demoted.") : BadRequest("Something went wrong");
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpGet("users")]
        public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20,
        [FromQuery] string? nickname = null,
        [FromQuery] string? login = null,
        [FromQuery] List<string>? roles = null)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            var result = await _accountService.GetUsersAsync(skip, take, nickname, login, roles);
            return Ok(result);
        }
    }
}
