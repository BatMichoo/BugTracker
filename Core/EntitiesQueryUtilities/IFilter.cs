using Core.Entities;
using System.Linq.Expressions;

namespace Core.EntitiesQueryUtilities
{
    public interface IFilter<T> where T : BaseModel
    {
        Expression<Func<T, bool>> Apply();
    }
}