using Core.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class BugUserConfiguration : IEntityTypeConfiguration<BugUser>
    {
        public void Configure(EntityTypeBuilder<BugUser> builder)
        {
            builder.Property(u => u.Name).IsRequired();

            builder
                .HasMany(u => u.SavedSearches)
                .WithOne(s => s.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(u => u.AssignedBugs)
                .WithOne(b => b.Assignee)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasMany(u => u.CreatedBugs)
                .WithOne(b => b.Creator)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
