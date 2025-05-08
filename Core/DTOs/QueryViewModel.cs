using Core.EntitiesQueryUtilities;

namespace Core.DTOs
{
    public class QueryViewModel<T> where T : class
    {
        public Dictionary<string, string> Filters { get; set; }
        public List<SortingViewModel> Sortings { get; set; }
        public PagingInfo PageInfo { get; set; }
        public List<T> Items { get; set; }
    }
}
