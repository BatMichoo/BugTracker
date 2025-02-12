using Core.Entities;

namespace Core.EntitiesQueryUtilities
{
    public interface IFilterFactory<TEntity, TFilterByEnum>
        where TEntity : BaseModel
        where TFilterByEnum : struct, Enum
    {
        List<IFilter<TEntity>> CreateFilters(string? filter = null);
        IFilter<TEntity> CreateFilter(TFilterByEnum filterBy, string? value = null);
    }
}
