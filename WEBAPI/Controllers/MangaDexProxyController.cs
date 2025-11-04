using ENTITIES.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangaDexProxyController : MyController
    {
        private readonly IMangaDexProxy _proxy;
        public MangaDexProxyController(IMangaDexProxy proxy)
        {
            _proxy = proxy;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string path)
        {
            if (this.ContextLogin is null)
                return Unauthorized("Invalid token.");

            if (string.IsNullOrEmpty(path))
                return BadRequest("Path is required.");

            var result = await _proxy.GetAsync(path, Request.Query);
            if (result.IsSucceed)
                return Content(result.Result!, "application/json");

            return BadRequest(result.ErrorMessage);
        }
    }
}
