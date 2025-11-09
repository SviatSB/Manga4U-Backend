using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface IHistoryRepository
    {
        Task<List<History>> GetUserHistoriesAsync(long userId);
        Task<History?> GetAsync(long userId, string mangaExternalId);
        Task AddAsync(History history);
        Task UpdateAsync(History history);
        Task<List<long>> GetLastMangaIdsAsync(long userId, int limit);
    }
}
