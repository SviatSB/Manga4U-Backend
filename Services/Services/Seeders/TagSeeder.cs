using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;

using Services.Interfaces;

namespace Services.Services.Seeders
{
    public class TagSeeder(IMangaDexService mangaDexService, ITagRepository tagRepository) : ISeeder
    {
        public async Task SeedAsync()
        {

            var existingIds = tagRepository.GetAllExternalId();

            var responseTags = await mangaDexService.GetTagsAsync();

            var tags = responseTags.Data
                .Where(t => !existingIds.Contains(t.id))
                .Select(t => new Tag
                {
                    TagExternalId = t.id,
                    Name = t.tag
                });

            await tagRepository.AddTagsAsync(tags);
        }
    }
}
