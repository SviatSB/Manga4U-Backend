using Domain.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Services.Interfaces;
using Services.Respones.Tags;

namespace Services.Services
{
    public class MangaDexService : IMangaDexService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public MangaDexService(HttpClient httpClient, IMemoryCache cache, IOptions<MemoryCacheEntryOptions> cacheOptions)
        {
            _httpClient = httpClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
        }

        public async Task<TagsListResponse> GetTagsAsync()
        {
            var josn = await _httpClient.GetStringAsync("manga/tag");
            return JsonConvert.DeserializeObject<TagsListResponse>(josn)!;

        }

        public async Task<ProxyResult> ProxyGetAsync(string path, IQueryCollection query)
        {
            string uri = BuildUri(path, query);

            if (_cache.TryGetValue(uri, out string? cached))
                return ProxyResult.Success(cached);

            var response = await _httpClient.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            _cache.Set(uri, result, _cacheOptions);

            if (response.IsSuccessStatusCode)
                return ProxyResult.Success(result);

            return ProxyResult.Failure($"{response.StatusCode}: {result}\non uri: {uri}");
        }

        private string BuildUri(string path, IQueryCollection query)
        {
            var baseUri = new Uri(_httpClient.BaseAddress!, path);

            if (query.Count == 0)
                return baseUri.ToString();

            return QueryHelpers.AddQueryString(
                baseUri.ToString(),
                query.ToDictionary(k => k.Key, v => v.Value.ToString())
                     .Where(kv => kv.Key != "path"));
        }
    }
}
