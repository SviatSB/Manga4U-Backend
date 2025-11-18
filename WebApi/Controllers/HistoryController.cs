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
            var user = await GetCurrentUserAsync();
            return Ok(await historyService.GetAllAsync(user.Id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateHistory([FromBody] UpdateHistoryDto dto)
        {
            //Проверяется только айди манги, а айди главы и номер главы - нет. Поэтому аккуратно с этим.

            var user = await GetCurrentUserAsync();

            var result = await historyService.UpdateHistoryAsync(user.Id, dto);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("recomendation-vector")]
        [Authorize]
        public async Task<IActionResult> Recomendation([FromQuery] int limit = 20)
        {
            var user = await GetCurrentUserAsync();
            return Ok(await historyService.GetRecomendationAsync(user.Id, limit));
        }
    }
}
