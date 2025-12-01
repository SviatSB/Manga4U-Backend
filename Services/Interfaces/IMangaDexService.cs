using Domain.Results;

using Microsoft.AspNetCore.Http;

using Services.Respones.Tags;

using Services.Results;

using static Services.DTOs.MangaDTOs.MangaDexMangaDto;

namespace Services.Interfaces
{
    public interface IMangaDexService
    {
        Task<Result<string>> ProxyGetAsync(string path, IQueryCollection query);
        Task<Result<RootResponse>> GetMangaAsync(string id);
        Task<TagsListResponse> GetTagsAsync();
    }
}
