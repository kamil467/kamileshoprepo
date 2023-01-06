using KamilCataLogAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KamilCataLogAPI.DBContext
{
    public class CatalogItemEntityConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("CatalogItem");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .IsRequired(false);

            builder.Property(x => x.Description)
                .HasColumnName("Description")
                .IsRequired(false);
            
            builder.Property(x => x.Price)
                .HasColumnName("Price")
                .IsRequired(false);

            builder.Property(x => x.AvailableStock)
                    .HasColumnName("AvailableStock")
                    .IsRequired(false);

            builder.Property(x => x.MaxStockThreshold)
                .HasColumnName("MaxStockThreshold")
                .IsRequired(false);

            builder.Property(x => x.RestockThreshold)
                .HasColumnName("RestockThreshold")
                .IsRequired(false);

            builder.Property(x => x.OnReorder)
                .HasColumnName("OnReorder")
                .IsRequired();

            builder.Property(x => x.BrandId)
                .HasColumnName("CatalogBrandId")
                .IsRequired(false);

            builder.Property(x => x.TypeId)
                .HasColumnName("CatalogTypeId")
                .IsRequired(false);

            builder.HasOne(x => x.CatalogType)
            .WithMany()
            .HasForeignKey(x => x.TypeId);

            builder.HasOne(x => x.CataLogBrand)
                    .WithMany()
                    .HasForeignKey(x => x.BrandId);

        }
    }
}
