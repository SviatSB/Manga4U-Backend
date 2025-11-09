using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.DTOs.AccountDTOs;
using Services.DTOs.HistoryDTOs;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController(IHistoryService historyService, IUserService userService) : MyController(userService)
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var user = GetCurrentUserAsync();
            return Ok(await historyService.GetAllAsync(user.Id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateHistory([FromBody] UpdateHistoryDto dto)
        {
            var user = GetCurrentUserAsync();
            await historyService.UpdateHistoryAsync(user.Id, dto);
            return Ok();
        }

        [HttpGet("recomendation-vector")]
        [Authorize]
        public async Task<IActionResult> Recomendation([FromQuery] int limit = 20)
        {
            var user = GetCurrentUserAsync();
            return Ok(await historyService.GetRecomendationAsync(user.Id, limit));
        }
    }
}
