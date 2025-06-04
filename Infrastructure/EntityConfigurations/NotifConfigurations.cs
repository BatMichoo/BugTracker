
using Core.Entities.NotifEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class NotifConfiguration : IEntityTypeConfiguration<Notif>
    {
        public void Configure(EntityTypeBuilder<Notif> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.AssignedById)
                .IsRequired();

            builder.Property(n => n.AssigneeId)
                .IsRequired();

            builder.Property(n => n.BugId)
                .IsRequired();

            builder.Property(n => n.IsRead)
                .HasDefaultValue(false);
        }
    }
}
