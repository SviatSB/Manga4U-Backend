namespace Services.Results.Base
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
    }
}
