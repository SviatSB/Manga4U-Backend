using DataInfrastructure.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class MangaRepository(MyDbContext myDbContext) : IMangaRepository
    {
        public async Task AddAsync(Manga manga)
        {
            await myDbContext.Mangas.AddAsync(manga);
            await myDbContext.SaveChangesAsync();
        }

        public async Task<Manga?> FindByExternalIdAsync(string id)
        {
            var res = await myDbContext.Mangas.SingleOrDefaultAsync(m => m.ExternalId == id);
            return res;
        }

        public async Task LinkTagsByExternalIdsAsync(Manga manga, IEnumerable<string> tagExternalIds)
        {
            if (manga == null) throw new ArgumentNullException(nameof(manga));
            if (tagExternalIds == null) throw new ArgumentNullException(nameof(tagExternalIds));

            // Attach manga if not tracked and ensure tags collection loaded
            myDbContext.Attach(manga);
            await myDbContext.Entry(manga).Collection(m => m.Tags).LoadAsync();

            var tagExternalIdsSet = tagExternalIds
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id => id.Trim())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (tagExternalIdsSet.Count == 0)
                return; // nothing to link

            // Fetch all tags that match provided external ids
            var tags = await myDbContext.Tags
                .Where(t => tagExternalIdsSet.Contains(t.TagExternalId))
                .ToListAsync();

            if (tags.Count == 0)
                return;

            // Link only those not already linked
            var existingLinkedTagIds = manga.Tags.Select(t => t.Id).ToHashSet();
            foreach (var tag in tags)
            {
                if (!existingLinkedTagIds.Contains(tag.Id))
                {
                    manga.Tags.Add(tag);
                }
            }

            await myDbContext.SaveChangesAsync();
        }

        public async Task<List<Tag>> GetTagsForMangasAsync(IEnumerable<long> mangaIds)
        {
            var ids = mangaIds?.ToArray() ?? Array.Empty<long>();
            if (ids.Length == 0) return new List<Tag>();

            // Select tags through the many-to-many relationship
            return await myDbContext.Mangas
                .Where(m => ids.Contains(m.Id))
                .SelectMany(m => m.Tags)
                .ToListAsync();
        }
    }
}
