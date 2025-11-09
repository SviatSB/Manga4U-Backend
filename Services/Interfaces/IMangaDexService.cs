using Domain.Results;

using Microsoft.AspNetCore.Http;

using Services.Respones.Tags;

using static Services.DTOs.MangaDTOs.MangaDexMangaDto;

namespace Services.Interfaces
{
    public interface IMangaDexService
    {
        Task<ProxyResult> ProxyGetAsync(string path, IQueryCollection query);
        Task<RootResponse> GetMangaAsync(string id);
        Task<TagsListResponse> GetTagsAsync();
    }
}
