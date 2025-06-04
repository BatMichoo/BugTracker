
using Core.Entities.NotifEntity;

namespace Core.Repositories
{
    public interface INotifRepository : IRepository<Notif>
    {
        Task<List<Notif>> GetUnRead(string userId);
        Task MarkRead(int Id);
    }
}
