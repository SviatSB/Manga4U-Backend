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
            var res = await _accountService.BanAsync(userId);

            if (res)
            {
                return Ok("User has been banned.");
            }
            else
            {
                return BadRequest("Something went wrong");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/unban")]
        public async Task<IActionResult> UnBan([FromRoute] long userId)
        {
            var res = await _accountService.UnBanAsync(userId);

            if (res)
            {
                return Ok("User has been unbanned.");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/mute")]
        public async Task<IActionResult> Mute([FromRoute] long userId)
        {
            var res = await _accountService.MuteAsync(userId);

            if (res)
            {
                return Ok("User has been muted.");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/unmute")]
        public async Task<IActionResult> UnMute([FromRoute] long userId)
        {
            var res = await _accountService.UnMuteAsync(userId);

            if (res)
            {
                return Ok("User has been unmuted.");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("user/{userId}/promote")]
        public async Task<IActionResult> Promote([FromRoute] long userId)
        {
            var res = await _accountService.PromoteAsync(userId);

            if (res)
            {
                return Ok("User has been promoted.");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("user/{userId}/demote")]
        public async Task<IActionResult> Demote([FromRoute] long userId)
        {
            var res = await _accountService.DemoteAsync(userId);

            if (res)
            {
                return Ok("User has been demoted.");
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }


    }
}
