using Core.Utilities;
using Infrastructure.Models;

namespace Core.Repository
{
    public interface IAdvancedRepository<T> : IRepository<T> where T : BaseEntity
    {        
        Task<IList<T>> RetrieveData(IList<IFilter<T>> filters, ISortingOptions<T> sortingOptions, string? searchTerm, PagingInfo? pagingInfo);
    }
}
