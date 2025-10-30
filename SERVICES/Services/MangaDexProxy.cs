using ENTITIES.Interfaces;
using ENTITIES.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;


namespace SERVICES.Services
{
    public class MangaDexProxy : IMangaDexProxy
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        public MangaDexProxy(HttpClient httpClient, IConfiguration config, IMemoryCache cache, MemoryCacheEntryOptions memoryCacheEntryOptions)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", config.GetSection("ProxyConfig").GetValue<string>("UserAgent"));
            _cache = cache;
            _memoryCacheEntryOptions = memoryCacheEntryOptions;
        }

        public async Task<ProxyResult> GetAsync(string path, IQueryCollection query)
        {
            string uri = BuildUri(path, query);

            if (_cache.TryGetValue(uri, out string? cached))
            {
                return ProxyResult.Success(cached);
            }

            var response = await _httpClient.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            _cache.Set(uri, result, _memoryCacheEntryOptions);

            try
            {
                response.EnsureSuccessStatusCode();
                return ProxyResult.Success(result);

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(response.ToString());
                return ProxyResult.Failure($"{ex.Message}\non uri: {uri}");
            }
        }

        private string BuildUri(string path, IQueryCollection query)
        {
            var baseUri = new Uri(_httpClient.BaseAddress!, path);

            string uri;
            if (query.Count > 0)
            {
                uri = QueryHelpers.AddQueryString(
                    baseUri.ToString(), query.ToDictionary(k => k.Key, v => v.Value.ToString())
                        .Where(kv => kv.Key != "path"));
            }
            else
            {
                uri = baseUri.ToString();
            }
            ;
            return uri;
        }
    }
}
