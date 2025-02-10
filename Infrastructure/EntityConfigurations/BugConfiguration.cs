using Core.Entities.BugEntity;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class BugConfiguration : IEntityTypeConfiguration<Bug>
    {
        public void Configure(EntityTypeBuilder<Bug> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(b => b.Priority)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(BugValidation.TitleMaxLength);

            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(BugValidation.DescMaxLength);

            builder.Property(b => b.CreatedOn)
                .IsRequired()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(b => b.CreatorId)
                .IsRequired()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.HasOne(b => b.Creator)
                .WithMany(c => c.CreatedBugs)
                .HasForeignKey(b => b.CreatorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Assignee)
                .WithMany(a => a.AssignedBugs)
                .HasForeignKey(b => b.AssigneeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(b => b.Comments)
                .WithOne(bc => bc.Bug)
                .HasForeignKey(c => c.BugId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
