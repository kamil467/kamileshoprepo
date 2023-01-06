using KamilCataLogAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KamilCataLogAPI.DBContext
{
    public class CatalogTypeEntityConfiguration : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");

            builder.HasKey(x => x.Id)
                    .HasName("Id");

            builder.Property(x => x.Name)
                   .HasColumnName("Name")
                   .IsRequired(false);

            builder.HasMany(x => x.CatalogItems)
               .WithOne();
        }
    }
}
