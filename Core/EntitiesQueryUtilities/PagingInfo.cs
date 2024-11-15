using Core.EntitiesQueryUtilities;

namespace Core.Utilities
{
    public class PagingInfo
    {
        public int CurrentPage { get; private set; }
        public int ElementsPerPage { get; private set; }
        public int PageCount => (int)Math.Ceiling(TotalElementCount / (double)ElementsPerPage);
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => TotalElementCount > CurrentPage * ElementsPerPage;

        public int TotalElementCount { get; internal set; }

        public static PagingInfo CreatePage(int totalElements = 0,
                                            int pageNumber = PagingDefaults.StartingPageNumber,
                                            int elementsPerPage = PagingDefaults.ElementsPerPage)
        {
            int pageCount = pageNumber;

            if (totalElements != 0)
            {
                pageCount = (int)Math.Ceiling(totalElements / (double)elementsPerPage);

                if (pageCount < pageNumber)
                {
                    pageCount = pageNumber;
                }
            }

            return new PagingInfo { CurrentPage = pageNumber, ElementsPerPage = elementsPerPage };
        }
    }
}
