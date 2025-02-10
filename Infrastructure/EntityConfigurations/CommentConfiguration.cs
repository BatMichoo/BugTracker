using Core.Entities.CommentEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(CommentValidation.MaxLength);

            builder.HasCheckConstraint($"CK_{nameof(Comment)}_{nameof(Comment.Likes)}_NonNegative", $"[{nameof(Comment.Likes)}] >= 0");

            builder.HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Replies)
                .WithOne(r => r.Comment)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.BugId)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(c => c.PostedOn)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Property(c => c.AuthorId)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
