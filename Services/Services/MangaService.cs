using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

using DataInfrastructure.Interfaces;

using Services.Interfaces;

namespace Services.Services
{
    public class MangaService(IMangaRepository mangaRepository, IMangaDexService mangaDexService) : IMangaService
    {
        public async Task<Manga> AddIfNotExist(string id)
        {
            var manga = await mangaRepository.FindByExternalIdAsync(id);

            if (manga == null)
            {
                var root = await mangaDexService.GetMangaAsync(id);
                var dto = root.Data;
                manga = new Manga()
                {
                    Name = dto.Name,
                    ExternalId = dto.Id
                };
                await mangaRepository.AddAsync(manga);

                await mangaRepository.LinkTagsByExternalIdsAsync(manga, dto.TagIds);
            }

            return manga;
        }
    }
}
