namespace Core.Utilities
{
    public class PagingInfo
    {
        public int CurrentPage { get; private set; }
        public int ElementsPerPage { get; private set; }

        public static PagingInfo CreatePage(int page, int elementsPerPage)
        {
            return new PagingInfo { CurrentPage = page, ElementsPerPage = elementsPerPage };
        }
    }
}
