using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
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

            return await GetById(entity.Id);
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

        public async Task<T?> GetById(int id)
        {
            var query = AddInclusions(AsQueryable());

            T? entity = await query
                .FirstOrDefaultAsync(b => b.Id == id);

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            dbSet.Update(entity);

            await SaveChangesAsync();

            return await GetById(entity.Id)!;
        }

        private async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        internal IQueryable<T> AsQueryable()
            => dbSet.AsQueryable();

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);

            await SaveChangesAsync();
        }

        internal abstract IQueryable<T> AddInclusions(IQueryable<T> query);

        public async Task<int> CountTotal()
        {
            var count = await dbSet.CountAsync();

            return count;
        }
    }
}
