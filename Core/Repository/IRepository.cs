namespace Core.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetById(int id);
        Task Create(T entity);
        Task Update(T entity);
        Task DeleteById(int id);
        Task SaveChangesAsync();

        IQueryable<T> AsQueryable();
    }
}
