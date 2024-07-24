using Core.Utilities.Bugs;
using Infrastructure.Models.Bug;

namespace Core.Repository
{
    public interface IAdvancedRepository<T> : IRepository<T> where T : class
    {
        Task<IList<Bug>> RetrieveData(IList<IFilter<T>> filters, BugSortingOptions sortingOptions, string? searchTerm, PagingInfo? pagingInfo);
    }
}
