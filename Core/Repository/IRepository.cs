namespace Core.Repository
{
    public interface IRepository<T> where T : class
    {
        abstract Task<T?> GetById(int id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task DeleteById(int id);
        Task Delete(T entity);
        Task SaveChangesAsync();

        IQueryable<T> AsQueryable();
    }
}
