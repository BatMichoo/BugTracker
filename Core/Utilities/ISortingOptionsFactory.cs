using Infrastructure.Models;

namespace Core.Utilities
{
    public interface ISortingOptionsFactory<T> where T : BaseEntity
    {
        ISortingOptions<T> CreateSortingOptions(string sortOptions);
    }
}
