using Domain.Results;

using Microsoft.AspNetCore.Http;

namespace Services.Interfaces
{
    public interface IMangaDexProxy
    {
        Task<ProxyResult> GetAsync(string path, IQueryCollection query);
    }
}
