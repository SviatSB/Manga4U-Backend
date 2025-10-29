using ENTITIES.Interfaces;
using ENTITIES.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;


namespace SERVICES.Services
{
    public class MangaDexProxy : IMangaDexProxy
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public MangaDexProxy(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", config.GetValue<string>("UserAgent"));
        }

        public async Task<ProxyResult> GetAsync(string path, IQueryCollection query)
        {
            var queryString = query.ToString();

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
            };

            var response = await _httpClient.GetAsync(uri);

            try
            {
                response.EnsureSuccessStatusCode();
                return ProxyResult.Success(await response.Content.ReadAsStringAsync());

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(response.ToString());
                return ProxyResult.Failure($"{ex.Message}\non uri: {uri}");
            }
        }
    }
}
