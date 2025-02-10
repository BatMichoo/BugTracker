using Core.Entities;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities
{
    public interface ISortingOptions<T> where T : BaseModel
    {
        public SortOrder SortOrder { get; }

        public Expression<Func<T, object>> Sort();
    }
}
