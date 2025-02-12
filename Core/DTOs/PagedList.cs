using Core.EntitiesQueryUtilities;

namespace Core.DTOs
{
    public class PagedList<T> where T : class
    {
        public PagingInfo PageInfo { get; set; } = null!;
        public List<T> Items { get; set; } = new List<T>();
    }
}
