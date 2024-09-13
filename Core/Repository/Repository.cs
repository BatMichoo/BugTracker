using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly TrackerDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(TrackerDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> Create(T entity)
        {
            await _dbSet.AddAsync(entity);

            await SaveChangesAsync();

            return await GetById(entity.Id);
        }

        public async Task DeleteById(int id)
        {
            T? entity = await GetById(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);

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

        public async Task<List<T>> GetAll()
        {
            var entityList = await AsQueryable().ToListAsync();

            return entityList;
        }

        public async Task<T> Update(T entity)
        {
            _dbSet.Update(entity);

            await SaveChangesAsync();

            return await GetById(entity.Id)!;
        }

        private async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        internal IQueryable<T> AsQueryable()
            => _dbSet.AsQueryable();

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);

            await SaveChangesAsync();
        }

        internal virtual IQueryable<T> AddInclusions(IQueryable<T> query)
            => query;

        public async Task<int> CountTotal()
        {
            var count = await _dbSet.CountAsync();

            return count;
        }

        public async Task<bool> DoesExist(int id)
            => await _dbSet.AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();
    }
}
