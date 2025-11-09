using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class TagRepository(MyDbContext myDbContext) : ITagRepository
    {
        public async Task AddTagsAsync(IEnumerable<Tag> tags)
        {
            await myDbContext.Tags.AddRangeAsync(tags);
            await myDbContext.SaveChangesAsync();
        }

        public List<string> GetAllExternalId()
        {
            return myDbContext.Tags.Select(x => x.TagExternalId).ToList();
        }
    }
}
