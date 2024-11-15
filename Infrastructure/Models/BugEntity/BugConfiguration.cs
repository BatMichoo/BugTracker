using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Models.BugEntity
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

            builder.Property(b => b.CreatedOn)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(b => b.CreatorId)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            
            builder.HasOne(b => b.Creator)
                .WithMany(c => c.CreatedBugs)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Assignee)
                .WithMany(a => a.AssignedBugs)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(b => b.Comments)
                .WithOne(bc => bc.Bug)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
