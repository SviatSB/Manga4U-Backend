using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface IMangaRepository : IRepository<Manga>
    {
        //Find
        Task<Manga?> FindByExternalIdAsync(string id);
        // Link existing tags (already stored in DB) to a manga using tag external ids
        Task LinkTagsByExternalIdsAsync(Manga manga, IEnumerable<string> tagExternalIds);
        // Get tags for mangas
        Task<List<Tag>> GetTagsForMangasAsync(IEnumerable<long> mangaIds);
    }
}
