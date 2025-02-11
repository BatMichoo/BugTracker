using Core.Entities;

namespace Core.EntitiesQueryUtilities
{
    public interface ISortingOptionsFactory<T, TOrderByEnum>
        where T : BaseModel
        where TOrderByEnum : Enum
    {
        IList<ISortingOptions<T>> CreateSortingOptions(string? sortOptions = null);
        ISortingOptions<T> CreateSortingOptions(SortOrder order, TOrderByEnum orderBy);
    }
}
