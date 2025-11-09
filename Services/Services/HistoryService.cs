using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Migrations;

using Services.DTOs.HistoryDTOs;
using Services.Interfaces;

namespace Services.Services
{
    public class HistoryService : IHistoryService
    {
        public Task<IEnumerable<GetHistoryDto>> GetAllAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<RecomendationDto>> GetRecomendationAsync(long userId, int limit)
        {
            throw new NotImplementedException();
        }

        public Task UpdateHistoryAsync(long userId, UpdateHistoryDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
