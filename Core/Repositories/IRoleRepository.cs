using Core.Entities.CustomRole;

namespace Core.Repositories
{
    public interface IRoleRepository
    {
        CustomRole GetRoleById(string id);
        CustomRole GetRoleByName(string name);
        List<CustomRole> GetAllRoles();
        bool Delete(string id);
        CustomRole Update(CustomRole updatingRole);
    }
}
