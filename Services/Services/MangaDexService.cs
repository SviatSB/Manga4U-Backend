using Domain.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Services.Interfaces;
using Services.Respones.Tags;

using Services.Results;

using static Services.DTOs.MangaDTOs.MangaDexMangaDto;

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

        public async Task<Result<RootResponse>> GetMangaAsync(string id)
        {
            try
            {
                var josn = await _httpClient.GetStringAsync($"manga/{id}");
                return Result<RootResponse>.Success(JsonConvert.DeserializeObject<RootResponse>(josn)!);
            }
            catch (HttpRequestException ex)
            {
                return Result<RootResponse>.Failure($"Error fetching manga with ID {id}: {ex.Message}");
            }
        }

        public async Task<TagsListResponse> GetTagsAsync()
        {
            var josn = await _httpClient.GetStringAsync("manga/tag");
            return JsonConvert.DeserializeObject<TagsListResponse>(josn)!;

        }

        public async Task<Result<string>> ProxyGetAsync(string path, IQueryCollection query)
        {
            string uri = BuildUri(path, query);

            if (_cache.TryGetValue(uri, out string? cached))
                return Result<string>.Success(cached);

            var response = await _httpClient.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            _cache.Set(uri, result, _cacheOptions);

            if (response.IsSuccessStatusCode)
                return Result<string>.Success(result);

            return Result<string>.Failure($"{response.StatusCode}: {result}\non uri: {uri}");
        }

        private string BuildUri(string path, IQueryCollection query)
        {
            var baseUri = new Uri(_httpClient.BaseAddress!, path);

            if (query.Count == 0)
                return baseUri.ToString();

            // Побудова URI з правильним форматуванням масивів
            var uriBuilder = new UriBuilder(baseUri);
            var queryBuilder = new System.Text.StringBuilder();

            bool firstParam = true;
            foreach (var kv in query)
            {
                if (kv.Key == "path")
                    continue;

                // Для масивів (параметри з []) передаємо кожне значення окремо
                if (kv.Key.Contains("[]"))
                {
                    foreach (var value in kv.Value)
                    {
                        if (!firstParam)
                            queryBuilder.Append('&');
                        queryBuilder.Append(Uri.EscapeDataString(kv.Key));
                        queryBuilder.Append('=');
                        queryBuilder.Append(Uri.EscapeDataString(value ?? string.Empty));
                        firstParam = false;
                    }
                }
                else
                {
                    if (!firstParam)
                        queryBuilder.Append('&');
                    queryBuilder.Append(Uri.EscapeDataString(kv.Key));
                    queryBuilder.Append('=');
                    queryBuilder.Append(Uri.EscapeDataString(kv.Value.ToString()));
                    firstParam = false;
                }
            }

            if (queryBuilder.Length > 0)
                uriBuilder.Query = queryBuilder.ToString();

            return uriBuilder.ToString();
        }
    }
}