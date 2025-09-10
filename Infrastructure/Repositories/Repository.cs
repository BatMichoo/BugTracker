using Core.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly TrackerDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public Repository(TrackerDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> Create(T entity)
        {
            await _dbSet.AddAsync(entity);

            await SaveChangesAsync();

            return (await GetById(entity.Id))!;
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

        public async Task<T?> GetById(int id, bool isFullyIncluded = false)
        {
            var query = AddInclusions(AsQueryable(), isFullyIncluded);

            T? entity = await query
                .FirstOrDefaultAsync(b => b.Id == id);

            return entity;
        }

        public virtual Task<List<T>> GetAll()
        {
            var entityList = _dbSet.AsNoTracking()
                .ToListAsync();

            return entityList;
        }

        protected IQueryable<T> AsQueryable()
            => _dbSet.AsQueryable();

        protected virtual IQueryable<T> AddInclusions(IQueryable<T> query, bool isFullyIncluded = false)
            => query;

        public async Task<T> Update(T updatedEntity)
        {
            T existingEntity = (await _dbSet.FindAsync(updatedEntity.Id))!;

            var entry = _dbContext.Entry(existingEntity);

            entry.CurrentValues.SetValues(updatedEntity);

            await SaveChangesAsync();

            return existingEntity;
        }

        private async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);

            await SaveChangesAsync();
        }

        public async Task<bool> DoesExist(int id)
            => await _dbSet.AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();
    }
}
