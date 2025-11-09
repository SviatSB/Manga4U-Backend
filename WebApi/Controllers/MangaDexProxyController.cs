using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangaDexProxyController : MyController
    {
        private readonly IMangaDexService _proxy;
        public MangaDexProxyController(IMangaDexService proxy, IUserService userService) : base(userService)
        {
            _proxy = proxy;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string path)
        {
            //if (this.ContextLogin is null)
            //    return Unauthorized("Invalid token.");

            if (string.IsNullOrEmpty(path))
                return BadRequest("Path is required.");

            var result = await _proxy.ProxyGetAsync(path, Request.Query);
            if (result.IsSucceed)
                return Content(result.Result!, "application/json");

            return BadRequest(result.ErrorMessage);
        }
    }
}
