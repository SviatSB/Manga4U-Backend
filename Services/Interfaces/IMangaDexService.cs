using Domain.Results;

using Microsoft.AspNetCore.Http;

using Services.Respones.Tags;

namespace Services.Interfaces
{
    public interface IMangaDexService
    {
        Task<ProxyResult> ProxyGetAsync(string path, IQueryCollection query);
        //Task<> GetMangaAsync(string id);
        Task<TagsListResponse> GetTagsAsync();
    }
}
