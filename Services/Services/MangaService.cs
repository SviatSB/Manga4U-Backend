using DataInfrastructure.Interfaces;

using Domain.Models;

using Services.Interfaces;
using Services.Results;

namespace Services.Services
{
    public class MangaService(IMangaRepository mangaRepository, IMangaDexService mangaDexService) : IMangaService
    {
        public async Task<Result<Manga>> AddIfNotExist(string id)
        {
            var manga = await mangaRepository.FindByExternalIdAsync(id);

            if (manga == null)
            {
                var getMangaResult = await mangaDexService.GetMangaAsync(id);
                if(!getMangaResult.IsSucceed)
                {
                    return Result<Manga>.Failure(getMangaResult.ErrorMessage);
                }

                var dto = getMangaResult.Value.Data;
                manga = new Manga()
                {
                    Name = dto.Name,
                    ExternalId = dto.Id
                };
                await mangaRepository.AddAsync(manga);

                await mangaRepository.LinkTagsByExternalIdsAsync(manga, dto.TagIds);
            }

            return Result<Manga>.Success(manga);
        }
    }
}
