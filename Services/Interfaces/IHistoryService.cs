using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Results;

using Services.DTOs.HistoryDTOs;

namespace Services.Interfaces
{
    public interface IHistoryService
    {
        Task<IEnumerable<GetHistoryDto>> GetAllAsync(long userId);
        Task<Result> UpdateHistoryAsync(long userId, UpdateHistoryDto dto);
        Task<List<RecomendationDto>> GetRecomendationAsync(long userId, int limit);

    }
}
