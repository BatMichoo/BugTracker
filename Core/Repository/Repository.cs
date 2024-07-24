using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly TrackerDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(TrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public async Task<T> Create(T entity)
        {
            await dbSet.AddAsync(entity);

            await SaveChangesAsync();

            return entity;
        }

        public async Task DeleteById(int id)
        {
            T? entity = await GetById(id);

            if (entity != null)
            {
                dbSet.Remove(entity);

                await SaveChangesAsync();
            }
        }

        public async virtual Task<T?> GetById(int id)
        {           
            T? entity = await dbSet.FindAsync(id);

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            dbSet.Update(entity);

            await SaveChangesAsync();

            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public IQueryable<T> AsQueryable()
            => dbSet.AsNoTracking();

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);

            await SaveChangesAsync();
        }
    }
}
