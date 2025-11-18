using Domain.Models;

using Services.Results;

namespace Services.Interfaces
{
    public interface IMangaService
    {
        Task<Result<Manga>> AddIfNotExist(string id);
    }
}
