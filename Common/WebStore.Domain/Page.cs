namespace WebStore.Domain
{
    public record Page<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount)
    {
        public int PagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    };

}
