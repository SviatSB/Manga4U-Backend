using ENTITIES.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : MyController
    {
        IAccountService _accountService;
        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

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
    }
}
