using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services;
using Services.Interfaces;
using Services.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionController(ICollectionService collectionService, IUserService userService) : MyController(userService)
    {
        [Authorize]
        [HttpGet("system")]
        public async Task<IActionResult> GetSystemCollections()
        {
            var user = await GetCurrentUserAsync();

            var result = await collectionService.GetUserSystemCollectionsAsync(user.Id);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(DtoConvertor.CreateCollectionShortDto(result.Value));
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserCollections()
        {
            var user = await GetCurrentUserAsync();

            var result = await collectionService.GetUserNonSystemCollectionsAsync(user.Id);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(DtoConvertor.CreateCollectionShortDto(result.Value));
        }

        [Authorize]
        [HttpGet("{collectionId}")]
        public async Task<IActionResult> GetCollection([FromRoute] long collectionId)
        {
            var user = await GetCurrentUserAsync();

            var result = await collectionService.GetCollectionWithContentAsync(user.Id, collectionId);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(DtoConvertor.CreateCollectionFullDto(result.Value));
        }
    }
}
