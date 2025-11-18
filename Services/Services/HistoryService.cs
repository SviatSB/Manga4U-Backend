using DataInfrastructure.Interfaces;

using Domain.Models;

using FluentValidation;

using Services.DTOs.HistoryDTOs;
using Services.DtoValidators;
using Services.Interfaces;
using Services.Results;

namespace Services.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IMangaRepository _mangaRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMangaService _mangaService;

        public HistoryService(IHistoryRepository historyRepository, IMangaRepository mangaRepository, IUserRepository userRepository, IMangaService mangaService)
        {
            _historyRepository = historyRepository;
            _mangaRepository = mangaRepository;
            _userRepository = userRepository;
            _mangaService = mangaService;
        }

        public async Task<IEnumerable<GetHistoryDto>> GetAllAsync(long userId)
        {
            // ensure user exists (optional strict validation)
            var user = await _userRepository.FindAsync(userId);
            if (user == null) return Enumerable.Empty<GetHistoryDto>();

            var histories = await _historyRepository.GetUserHistoriesAsync(userId);
            return histories.Select(h => new GetHistoryDto
            {
                MangaName = h.Manga.Name,
                MangaExternalId = h.MangaExternalId,
                LastChapterId = h.LastChapterId,
                Language = h.Language,
                LastChapterTitle = h.LastChapterTitle,
                LastChapterNumber = h.LastChapterNumber,
                UpdatedAt = h.UpdatedAt
            });
        }

        public async Task<Result> UpdateHistoryAsync(long userId, UpdateHistoryDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var validator = new UpdateHistoryDtoValidator();
            var validatorResult = await validator.ValidateAsync(dto);

            if (!validatorResult.IsValid)
            {
                var errors = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure($"Validation failed: {errors}");
            }

            // verify user exists
            var user = await _userRepository.FindAsync(userId);
            if (user == null)
                return Result.Failure("User not found.");

            // Ensure manga exists via service (handles external retrieval / creation)
            var addIfNotExistResult = await _mangaService.AddIfNotExist(dto.MangaExternalId);
            if (!addIfNotExistResult.IsSucceed)
                return Result.Failure($"Manga could not be added or found: {addIfNotExistResult.ErrorMessage}");

            var manga = addIfNotExistResult.Value;

            var existing = await _historyRepository.GetAsync(userId, dto.MangaExternalId);
            if (existing == null)
            {
                var history = new History
                {
                    UserId = userId,
                    MangaId = manga.Id,
                    MangaExternalId = dto.MangaExternalId,
                    LastChapterId = dto.LastChapterId,
                    Language = dto.Language,
                    LastChapterTitle = dto.LastChapterTitle,
                    LastChapterNumber = dto.LastChapterNumber,
                    UpdatedAt = DateTime.UtcNow
                };
                await _historyRepository.AddAsync(history);
            }
            else
            {
                //TODO это выглядит страшно. Можно было бы это спрятать в дтоконвертор.
                bool changed = existing.LastChapterId != dto.LastChapterId ||
                               existing.LastChapterNumber != dto.LastChapterNumber ||
                               existing.LastChapterTitle != dto.LastChapterTitle ||
                               !string.Equals(existing.Language, dto.Language, StringComparison.OrdinalIgnoreCase);
                if (changed)
                {
                    existing.LastChapterId = dto.LastChapterId;
                    existing.LastChapterNumber = dto.LastChapterNumber;
                    existing.LastChapterTitle = dto.LastChapterTitle;
                    existing.Language = dto.Language;
                    existing.UpdatedAt = DateTime.UtcNow;
                    await _historyRepository.UpdateAsync(existing);
                }
            }

            return Result.Success();
        }

        public async Task<List<RecomendationDto>> GetRecomendationAsync(long userId, int limit)
        {
            if (limit <= 0) limit = 20;

            // get distinct manga ids from last histories (limit applied at repository)
            var lastMangaIds = await _historyRepository.GetLastMangaIdsAsync(userId, limit);
            if (lastMangaIds.Count == 0) return new List<RecomendationDto>();

            var tags = await _mangaRepository.GetTagsForMangasAsync(lastMangaIds);
            if (tags.Count == 0) return new List<RecomendationDto>();

            // Only genre tags, group by external id
            var grouped = tags
                .Where(t => string.Equals(t.Group, "genre", StringComparison.OrdinalIgnoreCase))
                .GroupBy(t => new { t.TagExternalId, t.Name })
                .Select(g => new RecomendationDto
                {
                    GenreId = g.Key.TagExternalId,
                    Genre = g.Key.Name,
                    Number = g.Count()
                })
                .OrderByDescending(r => r.Number)
                .ToList();

            return grouped;
        }
    }
}
