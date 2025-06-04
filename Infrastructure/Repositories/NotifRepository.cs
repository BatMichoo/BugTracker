using Core.Entities.NotifEntity;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NotifRepository : Repository<Notif>, INotifRepository
    {
        public NotifRepository(TrackerDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<Notif>> GetUnRead(string userId) 
        {
            var notifs = await AsQueryable()
                .AsNoTracking()
                .Where(n => n.AssigneeId == userId && !n.IsRead)
                .ToListAsync();

            return notifs;
        }

        public async Task MarkRead(int id) 
        {
            var notif = await GetById(id);


            Console.WriteLine($"INPUT ID IS {id}");
            Console.WriteLine($"NOTIF IS {notif?.Id}");

            if (notif is not null) 
            {
                notif.IsRead = true;
                await Update(notif);
            }
        }
    }
}
