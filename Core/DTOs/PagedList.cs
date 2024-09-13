using Core.Utilities;

namespace Core.DTOs
{
    public class PagedList<T> where T : class
    {
        public PagingInfo PageInfo { get; set; } = null!;
        public int ResultItemCount => Items.Count;
        
        public IList<T> Items { get; set; } = new List<T>();
    }
}
