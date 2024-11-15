using Infrastructure.Models;

namespace Core.Utilities
{
    public interface IFilterFactory<TEntity, TFilterByEnum> 
        where TEntity : BaseEntity
        where TFilterByEnum : struct, Enum
    {
        IList<IFilter<TEntity>> CreateFilters(string? filter = null);
        IFilter<TEntity> CreateFilter(TFilterByEnum filterBy, string? value = null);
    }
}
