using Core.Entities.CustomRole;

namespace Core.Repositories
{
    public interface IRoleRepository
    {
        Task<CustomRole?> GetRoleById(string id);
        Task<CustomRole?> GetRoleByName(string name);
        Task<List<CustomRole>> GetAllRoles();
        Task<CustomRole?> Create(CustomRole newRole);
        Task<bool> Delete(string id);
        Task<CustomRole> Update(CustomRole updatingRole);
    }
}
