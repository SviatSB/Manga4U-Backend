using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.DTOs.AccountDTOs;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : MyController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
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
            Console.WriteLine("✅ TOKEN: " + token);
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (this.ContextLogin == null)
                return Unauthorized("Invalid token.");

            var result = await _accountService.ChangePasswordAsync(this.ContextLogin, dto.OldPassword, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password successfully changed.");
        }

        [Authorize]
        [HttpPatch("change-nickname")]
        public async Task<IActionResult> ChangeNickname([FromBody][MinLength(3)][MaxLength(16)][Required] string newNickname)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (this.ContextLogin == null)
                return Unauthorized("Invalid token.");

            bool result = await _accountService.ChangeNicknameAsync(this.ContextLogin, newNickname);

            if (!result)
                return BadRequest("Somthing went wrong");

            return Ok("Nickname successfully changed.");
        }

        [Authorize]
        [HttpPatch("change-avatar")]
        public async Task<IActionResult> ChangeAvatar([Required] IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (file.Length == 0)
                return BadRequest("Empty file.");

            if (this.ContextLogin == null)
                return Unauthorized("Invalid token.");

            bool result = await _accountService.ChangeAvatarAsync(this.ContextLogin, file);

            if (!result)
                return BadRequest("Somthing went wrong");

            return Ok("Avatar successfully changed.");
        }

        [Authorize]
        [HttpPatch("reset-avatar")]
        public async Task<IActionResult> ResetAvatar()
        {
            if (this.ContextLogin == null)
                return Unauthorized("Invalid token.");

            bool result = await _accountService.ResetAvatarAsync(this.ContextLogin);

            if (!result)
                return BadRequest("Не вдалося повернути дефолтний аватар.");

            return Ok("Аватар скинуто до стандартного.");
        }

        [Authorize]
        [HttpPatch("language")]
        public async Task<IActionResult> SetLanguage([FromBody][Required][StringLength(2, MinimumLength = 2)] string language)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (this.ContextLogin == null)
                return Unauthorized("Invalid token.");

            var ok = await _accountService.SetLanguageAsync(this.ContextLogin, language);
            return ok ? Ok("Language updated.") : BadRequest("Something went wrong");
        }

        [Authorize]
        [HttpPatch("about")]
        public async Task<IActionResult> SetAboutMyself([FromBody] string? about)
        {
            if (this.ContextLogin == null)
                return Unauthorized("Invalid token.");

            var ok = await _accountService.SetAboutMyselfAsync(this.ContextLogin, about);
            return ok ? Ok("About updated.") : BadRequest("Something went wrong");
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            if (this.ContextLogin == null)
                return Unauthorized("Invalid token.");

            var result = await _accountService.GetUserDtoAsync(this.ContextLogin);

            return Ok(result);
        }
    }
}
