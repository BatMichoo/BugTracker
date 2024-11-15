using Infrastructure.Models;

namespace Core.Utilities
{
    public interface ISortingOptionsFactory<T, TOrderByEnum>
        where T : BaseEntity
        where TOrderByEnum : Enum
    {
        ISortingOptions<T> CreateSortingOptions(string? sortOptions = null);
        ISortingOptions<T> CreateSortingOptions(SortOrder order, TOrderByEnum orderBy);
    }
}
