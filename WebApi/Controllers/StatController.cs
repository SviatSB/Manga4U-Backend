using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Interfaces;
using Services.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatController(IUserService userService, IStatService statService) : MyController(userService)
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActive([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await statService.GetActiveUserCount(user.Id, start, end);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("registrations")]
        public async Task<IActionResult> GetRegistrations([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await statService.GetRegistrationCount(user.Id, start, end);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("reviews")]
        public async Task<IActionResult> GetReviews([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await statService.GetReviewCount(user.Id, start, end);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("comments")]
        public async Task<IActionResult> GetComments([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await statService.GetCommentCount(user.Id, start, end);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("collections")]
        public async Task<IActionResult> GetCollections([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await statService.GetCollectionCount(user.Id, start, end);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("genres/average")]
        public async Task<IActionResult> GetAverageByGenres([FromBody] List<string>? genreExternalIds)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await statService.GetAvarageRating(user.Id, genreExternalIds);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);

            var dto = result.Value.Select(p => new { Genre = p.Item1, Average = p.Item2 });
            return Ok(dto);
        }
    }
}
