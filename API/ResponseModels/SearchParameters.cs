using Core.EntitiesQueryUtilities;

namespace API.ResponseModels
{
    public class SearchParameters
    {
        public List<string>? Filters { get; set; }
        public PagingInfo? PagingInfo { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortOptions { get; set; }
    }
}