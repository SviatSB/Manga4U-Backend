using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Models;

namespace DataInfrastructure.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task AddTagsAsync(IEnumerable<Tag> tags);
        List<string> GetAllExternalId();
    }
}
