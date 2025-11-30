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
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(MyDbContext myDbContext) : base(myDbContext) { }
        public async Task AddTagsAsync(IEnumerable<Tag> tags)
        {
            await _myDbContext.Tags.AddRangeAsync(tags);
            await _myDbContext.SaveChangesAsync();
        }

        public List<string> GetAllExternalId()
        {
            return _myDbContext.Tags.Select(x => x.TagExternalId).ToList();
        }
    }
}
