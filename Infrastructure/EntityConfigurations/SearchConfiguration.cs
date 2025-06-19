
using Core.Entities.SearchEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class SearchConfiguration : IEntityTypeConfiguration<Search>
    {
        public void Configure(EntityTypeBuilder<Search> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.QueryString)
                .IsRequired()
                .HasMaxLength(SearchValidation.MaxContentLenght);

            builder.HasOne(r => r.CreatedBy)
                .WithMany(c => c.SavedSearches)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
