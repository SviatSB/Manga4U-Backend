using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
        public CollectionRepository(MyDbContext myDbContext) : base(myDbContext) { }

        public Task<IEnumerable<Collection>> GetUserCollectionsAsync(long userId)
        {
            var list = _myDbContext.Collections.Where(c => c.UserId == userId).AsEnumerable();
            return Task.FromResult(list);
        }

        public async Task<Collection?> GetWithMangasAsync(long id)
        {
            return await _myDbContext.Collections
                .Include(c => c.Mangas)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public Task<IEnumerable<Collection>> GetUserSystemCollectionsAsync(long userId)
        {
            var list = _myDbContext.Collections
                .Where(c => c.UserId == userId && c.SystemCollectionType != null)
                .AsEnumerable();
            return Task.FromResult(list);
        }

        public Task<IEnumerable<Collection>> GetUserNonSystemCollectionsAsync(long userId)
        {
            var list = _myDbContext.Collections
                .Where(c => c.UserId == userId && c.SystemCollectionType == null)
                .AsEnumerable();
            return Task.FromResult(list);
        }

        public async Task<IEnumerable<Collection>> SearchPublicCollectionsByNameAsync(string queryString)
        {
            // Пустой запрос → пустой результат
            if (string.IsNullOrWhiteSpace(queryString))
                return Enumerable.Empty<Collection>();

            // Нормализация и токенизация
            var words = Analyze(queryString);
            if (words.Length == 0)
                return Enumerable.Empty<Collection>();

            // Only public collections
            var allPublic = await _myDbContext.Collections
                .Where(c => c.IsPublic)
                .ToListAsync();

            // Взяли нормализованные слова
            var loweredWords = words.Select(w => w.ToLowerInvariant()).ToArray();

            // Фильтр: хотя бы одно слово совпадает
            var candidates = allPublic
                .Where(c =>
                    c.Name != null &&
                    loweredWords.Any(w => Normalize(c.Name).Contains(w))
                )
                .ToList();

            // Сортировка по количеству совпадений
            var ordered = candidates
                .OrderByDescending(c =>
                    loweredWords.Count(w => Normalize(c.Name).Contains(w))
                )
                .ToList();

            return ordered;
        }

        private static string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return "";

            // lower + удаление диакритики
            var lower = s.ToLowerInvariant();
            var decomposed = lower.Normalize(NormalizationForm.FormD);

            var filtered = new string(
                decomposed.Where(ch =>
                    CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark
                ).ToArray()
            );

            return filtered.Normalize(NormalizationForm.FormC);
        }

        private static string[] Analyze(string text)
        {
            var norm = Normalize(text);

            // Разбиваем на слова по не-буквенным символам
            var tokens = Regex
                .Split(norm, "\\W+")
                .Where(t => !string.IsNullOrWhiteSpace(t) && t.Length > 1) // >1 символ
                .Distinct()
                .ToArray();

            return tokens;
        }

        public async Task<Collection?> GetByIdWithContentAsync(long collectionId)
        {
            return await _myDbContext.Collections
                .Include(c => c.Mangas)
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == collectionId);
        }
    }
}
