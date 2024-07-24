namespace Core.DTOs.Bug
{
    public class PagedList<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PagesCount => (int) Math.Ceiling(ItemCount / (double) PageSize);
        public int ItemCount => Items.Count;
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => PagesCount > CurrentPage;
        public IList<T> Items { get; set; } = new List<T>();
    }
}
