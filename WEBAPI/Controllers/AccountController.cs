using ENTITIES;
using ENTITIES.DTOs.AccountDTOs;
using ENTITIES.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SERVICES.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : MyController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, JwtConfig jwtConfig)
        {
            this._accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.RegisterAsync(dto.Login, dto.Password, dto.Nickname);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _accountService.LoginAsync(dto.Login, dto.Password);
            if (token == null)
                return Unauthorized("Invalid login or password.");

            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _accountService.ChangePasswordAsync(dto.Login, dto.OldPassword, dto.NewPassword);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                return Ok("Password successfully changed.");
        }


    }
}
