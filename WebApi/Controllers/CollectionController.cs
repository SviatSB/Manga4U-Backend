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

        [HttpGet("{collectionId}")]
        public async Task<IActionResult> GetCollection([FromRoute] long collectionId)
        {
            var user = await GetCurrentUserAsync();

            var result = await collectionService.GetCollectionWithContentAsync(user?.Id, collectionId);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(DtoConvertor.CreateCollectionFullDto(result.Value));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCollection([FromBody] string name)
        {
            var user = await GetCurrentUserAsync();

            var result = await collectionService.CreateCollectionAsync(user.Id, name);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete("{collectionId}")]
        public async Task<IActionResult> DeleteCollection([FromRoute] long collectionId)
        {
            var user = await GetCurrentUserAsync();

            var result = await collectionService.DeleteCollectionAsync(user.Id, collectionId);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("{collectionId}/clone")]
        public async Task<IActionResult> CloneCollection([FromRoute] long collectionId)
        {
            var user = await GetCurrentUserAsync();

            var result = await collectionService.CopyPublicCollectionAsync(user.Id, collectionId);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

           return Ok(DtoConvertor.CreateCollectionShortDto(result.Value));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCollection([FromQuery] string name)
        {
            var result = await collectionService.SearchPublicCollectionsByNameAsync(name);

            if (!result.IsSucceed)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(DtoConvertor.CreateCollectionShortDto(result.Value));
        }

        [Authorize]
        [HttpPost("{collectionId}/visibility")]
        public async Task<IActionResult> SetVisibility([FromRoute] long collectionId, [FromBody] bool isPublic)
        {
            var user = await GetCurrentUserAsync();
            var result = await collectionService.SetCollectionPublicAsync(user.Id, collectionId, isPublic);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }

        [Authorize]
        [HttpPost("{collectionId}/rename")]
        public async Task<IActionResult> Rename([FromRoute] long collectionId, [FromBody] string newName)
        {
            var user = await GetCurrentUserAsync();
            var result = await collectionService.RenameCollectionAsync(user.Id, collectionId, newName);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }

        [Authorize]
        [HttpPost("{collectionId}/manga")]
        public async Task<IActionResult> AddManga([FromRoute] long collectionId, [FromQuery] string mangaExternalId)
        {
            var user = await GetCurrentUserAsync();
            var result = await collectionService.AddMangaToCollectionAsync(user.Id, collectionId, mangaExternalId);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{collectionId}/manga")]
        public async Task<IActionResult> RemoveManga([FromRoute] long collectionId, [FromQuery] string mangaExternalId)
        {
            var user = await GetCurrentUserAsync();
            var result = await collectionService.RemoveMangaFromCollectionAsync(user.Id, collectionId, mangaExternalId);
            if (!result.IsSucceed) return BadRequest(result.ErrorMessage);
            return Ok();
        }

    }
}
