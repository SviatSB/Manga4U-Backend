using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : MyController
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/ban")]
        public async Task<IActionResult> Ban([FromRoute] long userId)
        {
            throw new NotImplementedException();

            return Ok("User has been banned.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/unban")]
        public async Task<IActionResult> UnBan([FromRoute] long userId)
        {
            throw new NotImplementedException();

            return Ok("User has been unbanned.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/mute")]
        public async Task<IActionResult> Mute([FromRoute] long userId)
        {
            throw new NotImplementedException();

            return Ok("User has been mutted.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("user/{userId}/mute")]
        public async Task<IActionResult> UnMute([FromRoute] long userId)
        {
            throw new NotImplementedException();

            return Ok("User has been unmutted.");
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("/admin/users/{id}/promote")]
        public async Task<IActionResult> Promote([FromRoute] long userId)
        {
            throw new NotImplementedException();

            return Ok("User has been promoted.");
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("/admin/users/{id}/demote")]
        public async Task<IActionResult> Demote([FromRoute] long userId)
        {
            throw new NotImplementedException();

            return Ok("User has been demoted.");
        }


    }
}
