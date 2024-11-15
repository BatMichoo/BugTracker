using Infrastructure.Models;
using System.Linq.Expressions;

namespace Core.Utilities
{
    public interface IFilter<T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Apply();
    }
}