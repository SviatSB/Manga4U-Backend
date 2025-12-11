using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Services.Interfaces;
using Services.Respones.Tags;
using Services.Results.Base;

using static Services.DTOs.MangaDTOs.MangaDexMangaDto;

namespace Services.Services
{
    public class MangaDexService : IMangaDexService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        private readonly ILogger<MangaDexService> _logger;

        public MangaDexService(HttpClient httpClient, IMemoryCache cache, IOptions<MemoryCacheEntryOptions> cacheOptions, ILogger<MangaDexService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _cacheOptions = cacheOptions.Value;
            _logger = logger;
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
            _logger.LogInformation("Proxy request: {Uri}", uri);

            if (false && _cache.TryGetValue(uri, out string? cached))
            {
                _logger.LogInformation("Cache hit for {Uri}", uri);
                return Result<string>.Success(cached);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            // Удаляем опасные заголовки
            request.Headers.Remove("Referer");
            request.Headers.Remove("Origin");

            _logger.LogInformation("Outgoing request headers:");

            foreach (var header in request.Headers)
                _logger.LogInformation("  {Key}: {Value}", header.Key, string.Join(", ", header.Value));

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            _logger.LogInformation("Response status: {Status}", response.StatusCode);

            _logger.LogInformation("Response headers:");
            foreach (var header in response.Headers)
                _logger.LogInformation("  {Key}: {Value}", header.Key, string.Join(", ", header.Value));

            _logger.LogInformation("Response content headers:");
            foreach (var ch in response.Content.Headers)
                _logger.LogInformation("  {Key}: {Value}", ch.Key, string.Join(", ", ch.Value));

            var contentType = response.Content.Headers.ContentType?.MediaType;
            _logger.LogInformation("Content-Type: {ContentType}", contentType);

            var buffer = await response.Content.ReadAsByteArrayAsync();

            bool isText = contentType != null &&
                          (contentType.Contains("json") ||
                           contentType.Contains("text") ||
                           contentType.Contains("javascript") ||
                           contentType.Contains("xml"));

            string result;

            if (isText)
            {
                result = System.Text.Encoding.UTF8.GetString(buffer);
                _logger.LogInformation("Received TEXT content (length {Len})", result.Length);
            }
            else
            {
                result = Convert.ToBase64String(buffer);
                _logger.LogInformation("Received BINARY content (bytes {Len}), returned as base64", buffer.Length);
            }

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