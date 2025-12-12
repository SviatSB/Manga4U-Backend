using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
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

            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            request.Headers.Remove("Referer");
            request.Headers.Remove("Origin");

            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            _cache.Set(uri, result, _cacheOptions);

            if (response.IsSuccessStatusCode)
                return Result<string>.Success(result);

            return Result<string>.Failure($"{response.StatusCode}: {result}\non uri: {uri}");
        }

        public async Task<(byte[]? Bytes, string? ContentType, string? Error)> ProxyImageAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return (null, null, "Missing URL");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, imageUrl);

                // Удаляем заголовки, которые могут вызвать блокировку hotlinking
                request.Headers.Remove("Referer");
                request.Headers.Remove("Origin");

                // User-Agent уже задан при регистрации клиента — повторно не добавляем

                // Важно: использовать ResponseHeadersRead, чтобы не буферизовать полностью
                var response = await _httpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead
                );

                if (!response.IsSuccessStatusCode)
                    return (null, null, $"{response.StatusCode}");

                // Читаем байты (можно заменить на stream если хочешь стримить дальше)
                var bytes = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType
                                  ?? "application/octet-stream";

                return (bytes, contentType, null);
            }
            catch (Exception ex)
            {
                return (null, null, ex.Message);
            }
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