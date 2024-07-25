using Infrastructure.Models;
using System.Linq.Expressions;

namespace Core.Utilities
{
    public interface ISortingOptions<T> where T : BaseEntity
    {
        public SortOrder SortOrder { get; }

        public Expression<Func<T, object>> Sort();
    }
}
