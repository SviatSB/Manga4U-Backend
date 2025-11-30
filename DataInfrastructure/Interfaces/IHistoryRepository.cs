using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

using Microsoft.EntityFrameworkCore.Migrations;

namespace DataInfrastructure.Interfaces
{
    public interface IHistoryRepository : IRepository<History>
    {
        Task<List<History>> GetUserHistoriesAsync(long userId);
        Task<History?> GetAsync(long userId, string mangaExternalId);
        Task UpdateAsync(History history);
        Task<List<long>> GetLastMangaIdsAsync(long userId, int limit);
    }
}
