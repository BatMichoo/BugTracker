using Core.QueryBuilders;
using Core.QueryParameters;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly TrackerDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private readonly IQueryableBuilder<T> _queryableBuilder;

        public Repository(TrackerDbContext dbContext, IQueryableBuilder<T> queryableBuilder)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
            _queryableBuilder = queryableBuilder;
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

        public async Task<T?> GetById(int id)
        {
            var query = AddInclusions(AsQueryable());

            T? entity = await query
                .FirstOrDefaultAsync(b => b.Id == id);

            return entity;
        }

        public async Task<List<T>> ExecuteQuery(QueryParameters<T> queryParameters)
        {
            var query = _queryableBuilder.BuildQuery(AsQueryable(), queryParameters);

            var entityList = await query.ToListAsync();

            return entityList;
        }

        public async Task<T> Update(T entity)
        {
            T existingEntity = (await _dbSet.FindAsync(entity.Id))!;

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            await SaveChangesAsync();

            return entity;
        }

        private async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<T> AsQueryable()
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
