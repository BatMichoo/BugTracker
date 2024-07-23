using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TrackerDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(TrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task DeleteById(int id)
        {
            T? entity = await GetById(id);

            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public async Task<T?> GetById(int id)
        {
            T? entity = await dbSet.FindAsync(id);

            return entity;
        }

        public Task Update(T entity)
        {
            dbSet.Update(entity);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public IQueryable<T> AsQueryable()
            => dbSet.AsQueryable().AsNoTracking();
    }
}
