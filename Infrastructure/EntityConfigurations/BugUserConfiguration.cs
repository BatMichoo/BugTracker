using Core.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class BugUserConfiguration : IEntityTypeConfiguration<BugUser>
    {
        public void Configure(EntityTypeBuilder<BugUser> builder)
        {
            builder.Property(u => u.Name)
                .IsRequired();
        }
    }
}
