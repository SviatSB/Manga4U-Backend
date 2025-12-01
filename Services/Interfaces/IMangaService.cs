using Domain.Models;

using Services.Results.Base;

namespace Services.Interfaces
{
    public interface IMangaService
    {
        Task<Result<Manga>> GetOrAdd(string id);
    }
}
