using KamilCataLogAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KamilCataLogAPI.DBContext
{
    public class CatalogBrandEntityCOnfiguration : IEntityTypeConfiguration<CataLogBrand>
    {
        public void Configure(EntityTypeBuilder<CataLogBrand> builder)
        {
            builder.ToTable("CatalogBrand");

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
