using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services;
using Services.DTOs.OtherDTOs;
using Services.DTOs.ReviewDTOs;
using Services.Interfaces;
using Services.Results.Base;
using Services.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController(IReviewService reviewService, IUserService userService) : MyController(userService)
    {
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto req)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await reviewService.AddReviewAsync(user.Id, req.MangaExternalId, req.Stars, req.Text);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview([FromRoute] long reviewId)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await reviewService.DeleteReviewAsync(user.Id, reviewId);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }

        [HttpGet("manga/{mangaExternalId}")]
        public async Task<IActionResult> GetReviews([FromRoute] string mangaExternalId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var result = await reviewService.GetReviewsByMangaAsync(mangaExternalId, skip, take);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);

            var dto = new ReviewPagedDto
            {
                TotalCount = result.Value.TotalCount,
                Items = DtoConvertor.CreateReviewDto(result.Value.Items)
            };

            return Ok(dto);
        }

        [HttpGet("manga/{mangaExternalId}/avg")]
        public async Task<IActionResult> GetAverage([FromRoute] string mangaExternalId)
        {
            var avg = await reviewService.GetAverageStarsAsync(mangaExternalId);
            return Ok(new { Average = avg });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("pin")]
        public async Task<IActionResult> Pin([FromBody] PinDto dto)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await reviewService.SetPinnedStatusAsync(user.Id, dto.Id, dto.State);

            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }
    }
}
