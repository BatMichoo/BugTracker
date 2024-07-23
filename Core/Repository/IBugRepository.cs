using System.Linq.Expressions;

namespace Core.Repository
{
    public interface IBugRepository<T> : IRepository<T> where T : class
    {
        Task<IQueryable<T>> ApplyFilters(string searchTerm, string sortTerm, params Expression<Func<T, bool>>[] filters);
    }
}
