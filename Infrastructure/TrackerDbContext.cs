using Core.Entities.BugEntity;
using Core.Entities.CommentEntity;
using Core.Entities.ReplyEntity;
using Core.Entities.UserEntity;
using Core.Entities.NotifEntity;
using Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TrackerDbContext : IdentityDbContext<BugUser>
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) 
        {
        }

        public DbSet<Bug> Bugs { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Reply> Replies { get; set; } = null!;
        public DbSet<Notif> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BugConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ReplyConfiguration());
            modelBuilder.ApplyConfiguration(new BugUserConfiguration());
            modelBuilder.ApplyConfiguration(new NotifConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
