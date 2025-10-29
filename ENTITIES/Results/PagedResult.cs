using System.Collections.Generic;

namespace ENTITIES.Results
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
    }
}
