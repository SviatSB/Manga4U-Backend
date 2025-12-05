using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    public class TestController(IMangaDexService mangaDexService, IMangaService mangaService, IUserService userService) : MyController(userService)
    {
        [HttpPost("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        [HttpPost("tagtest")]
        public async Task<IActionResult> TagTest()
        {
            var res = await mangaDexService.GetTagsAsync();

            return Ok(res);
        }

        [HttpPost("mangatest")]
        public async Task<IActionResult> MangaTest()
        {
            var res = await mangaDexService.GetMangaAsync("cbf174ca-af25-4410-82fa-498a6df9ad3c");
            return Ok(res);
        }

        [HttpPost("mangatest2")]
        public async Task<IActionResult> MangaTest2()
        {
            await mangaService.GetOrAdd("cbf174ca-af25-4410-82fa-498a6df9ad3c");
            return Ok();
        }

        [HttpGet("usertest")]
        public async Task<IActionResult> UserTest()
        {
            return Ok(await this.GetCurrentUserAsync());
        }

        [Authorize]
        [HttpGet("debug-claims")]
        public IActionResult DebugClaims()
        {
            var dict = User.Claims
                .Select(c => new { c.Type, c.Value });

            return Ok(dict);
        }

    }
}
