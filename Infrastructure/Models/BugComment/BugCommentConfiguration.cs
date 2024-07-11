using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Models.BugComment
{
    public class BugCommentConfiguration : IEntityTypeConfiguration<BugComment>
    {
        public void Configure(EntityTypeBuilder<BugComment> builder)
        {
            builder.HasKey(bc => new { bc.BugId, bc.CommentId });
        }
    }
}
