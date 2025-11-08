using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    public class TestController(IJwtTokenGenerator generator) : Controller
    {

        [Authorize]
        [HttpPost("api/test1")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
