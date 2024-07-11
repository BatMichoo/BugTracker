using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Models.Bug
{
    public class BugConfiguration : IEntityTypeConfiguration<Bug>
    {
        public void Configure(EntityTypeBuilder<Bug> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Status)
                .HasConversion<int>();

            builder.Property(b => b.Priority)
                .HasConversion<int>();

            builder.HasOne(b => b.Creator)
                .WithMany(c => c.CreatedBugs)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Assignee)
                .WithMany(a => a.AssignedBugs)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(b => b.Comments)
                .WithOne(bc => bc.Bug)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
