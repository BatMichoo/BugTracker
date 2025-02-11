using Core.DTOs;

namespace API.ResponseModels
{
    public class SearchResponseViewModel<T> where T : class
    {
        public SearchParameters SearchParameters { get; set; }
        public PagedList<T>? PagedList { get; set; }
    }
}
