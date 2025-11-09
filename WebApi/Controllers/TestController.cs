using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    public class TestController(IMangaDexService mangaDexService) : Controller
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
    }
}
