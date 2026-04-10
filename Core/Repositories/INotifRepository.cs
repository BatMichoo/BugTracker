using Core.Entities.NotifEntity;

namespace Core.Repositories
{
    public interface IBugNotificationRepository : IRepository<BugNotification>
    {
        Task<List<BugNotification>> GetUnRead(string userId);
        Task MarkRead(int Id);
    }

    public interface IRoleNotificationRepository : IRepository<RoleNotification>
    {
        Task<List<RoleNotification>> GetUnRead(string userId);
        Task MarkRead(int Id);
    }
}
