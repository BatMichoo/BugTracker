
using Core.Entities.NotifEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class NotifConfiguration : IEntityTypeConfiguration<BugNotification>
    {
        public void Configure(EntityTypeBuilder<BugNotification> builder)
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
