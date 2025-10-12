using ENTITIES.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Authorization;

namespace WEBAPI.Controllers
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
