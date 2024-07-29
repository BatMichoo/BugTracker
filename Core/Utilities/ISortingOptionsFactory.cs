using Infrastructure.Models;

namespace Core.Utilities
{
    public interface ISortingOptionsFactory<T, U> where T : BaseEntity
        where U : Enum
    {
        ISortingOptions<T> CreateSortingOptions(string? sortOptions);
        ISortingOptions<T> CreateSortingOptions(SortOrder order, U orderBy);
    }
}
