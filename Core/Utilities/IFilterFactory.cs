using Core.Utilities.Comments;
using Infrastructure.Models;

namespace Core.Utilities
{
    public interface IFilterFactory<T> where T : BaseEntity
    {
        Task<IList<IFilter<T>>> CreateFilters(string? filter);
        Task<IFilter<T>> CreateFilter(CommentFilterType filterBy, object value);
    }
}
