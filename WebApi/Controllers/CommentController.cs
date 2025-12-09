using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services;
using Services.DTOs.CommentDTOs;
using Services.DTOs.OtherDTOs;
using Services.Interfaces;
using Services.Results.Custom;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController(ICommentService commentService, IUserService userService) : MyController(userService)
    {
        [HttpGet("root")]
        public async Task<IActionResult> GetRootComments([FromQuery] string chapterId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var result = await commentService.GetRootCommentsAsync(take, skip, chapterId);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);

            var items = DtoConvertor.CreateCommentDto(result.Value.Items);

            // if reply counts available, apply to DTOs
            if (result.Value.ReplyCounts != null && result.Value.ReplyCounts.Count > 0)
            {
                foreach (var itemDto in items)
                {
                    if (result.Value.ReplyCounts.TryGetValue(itemDto.Id, out var rc))
                        itemDto.ReplyCount = rc;
                }
            }

            var totalReplies = result.Value.ReplyCounts?.Values.Sum() ?? 0;

            var responseDto = new CommentPagedDto
            {
                TotalCount = result.Value.TotalCount,
                ReplyCount = totalReplies,
                Items = items
            };

            return Ok(responseDto);
        }

        [HttpGet("{commentId}/replies")]
        public async Task<IActionResult> GetReplies([FromRoute] long commentId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var result = await commentService.GetCommentRepliesAsync(take, skip, commentId);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);

            var items = DtoConvertor.CreateCommentDto(result.Value.Items);

            var totalReplies = result.Value.ReplyCounts?.Values.Sum() ?? 0;

            var responseDto = new CommentPagedDto
            {
                TotalCount = result.Value.TotalCount,
                ReplyCount = totalReplies,
                Items = items
            };

            return Ok(responseDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto req)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await commentService.CreateCommentAsync(user.Id, req.MangaChapterExternalId, req.Text, req.ParentCommentId);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] long commentId)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await commentService.DeleteCommentAsync(user.Id, commentId);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("pin")]
        public async Task<IActionResult> Pin([FromBody] PinDto dto)
        {
            var user = await GetCurrentUserAsync();
            if (user is null) return Unauthorized();

            var result = await commentService.SetPinnedStatusAsync(user.Id, dto.Id, dto.State);

            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }
    }
}
