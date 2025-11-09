using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface IMangaRepository
    {
        //Find
        Task<Manga?> FindByExternalIdAsync(string id);
        //Add
        Task AddAsync(Manga manga);
        // Link existing tags (already stored in DB) to a manga using tag external ids
        Task LinkTagsByExternalIdsAsync(Manga manga, IEnumerable<string> tagExternalIds);
    }
}
