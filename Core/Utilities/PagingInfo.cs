namespace Core.Utilities
{
    public class PagingInfo
    {
        public int CurrentPage { get; private set; }
        public int ElementsPerPage { get; private set; }
        public int PageCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => PageCount > CurrentPage;

        public int TotalElementCount { get; internal set; }

        public static PagingInfo CreatePage(int totalElements = 0, int page = 1, int elementsPerPage = 25)
        {
            int pageCount = (int) Math.Ceiling(totalElements / (double)elementsPerPage);

            if (pageCount < page) 
            {
                page = pageCount;
            }

            return new PagingInfo { CurrentPage = page, ElementsPerPage = elementsPerPage, PageCount = pageCount};
        }
    }
}
