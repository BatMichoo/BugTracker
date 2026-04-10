using Core.Entities.CustomRole;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly TrackerDbContext _dbContext;
        private readonly DbSet<CustomRole> _dbSet;

        public RoleRepository(TrackerDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Roles;
        }

        public async Task<CustomRole> Create(CustomRole newRole)
        {
            await _dbSet.AddAsync(newRole);
            await _dbContext.SaveChangesAsync();

            var createdRole = await _dbSet.AsNoTracking().FirstOrDefaultAsync(r => r.Id == newRole.Id);

            return createdRole;
        }

        public async Task<bool> Delete(string id)
        {
            var entry = await _dbSet.FirstOrDefaultAsync(r => r.Id == id && r.IsDeletable);

            if (entry is null)
            {
                return false;
            }

            _dbSet.Remove(entry);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<CustomRole>> GetAllRoles(string userId)
        {
            var roles = await _dbContext.UserRoles.Where(u => u.UserId == userId)
                .Select(r => new CustomRole { Id = r.RoleId })
                .ToListAsync();

            return roles;
        }

        public async Task<CustomRole> GetRoleById(string id)
        {
            var role = await _dbSet.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

            return role;
        }

        public async Task<CustomRole?> GetRoleByName(string name)
        {
            var role = await _dbSet.AsNoTracking().FirstOrDefaultAsync(r => r.Name == name);

            return role;
        }

        public async Task<CustomRole?> Update(CustomRole updatingRole)
        {
            var oldRole = await _dbSet.FirstOrDefaultAsync(r => r.Id == updatingRole.Id);

            if (oldRole is null)
            {
                return oldRole;
            }

            _dbSet.Entry(oldRole).CurrentValues.SetValues(updatingRole);

            await _dbContext.SaveChangesAsync();

            return oldRole;
        }
    }
}
