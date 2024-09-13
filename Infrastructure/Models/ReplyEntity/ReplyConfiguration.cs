using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Models.ReplyEntity
{
    public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Author);

            builder.HasOne(r => r.Comment)
                .WithMany(c => c.Replies)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
