using KamilCataLogAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace KamilCataLogAPI.DBContext
{
    /// <summary>
    /// CatalogDBContext Class.
    /// </summary>
    public class CatalogDBContext:DbContext
    {
        /// <summary>
        /// Get or Set CatalogItems.
        /// </summary>
        public DbSet<CatalogItem> CatalogItems { get; set; }

        /// <summary>
        /// Get or Set CatalogTypes.
        /// </summary>
        public DbSet<CatalogType> CatalogTypes { get; set; }

        /// <summary>
        /// Get or Set CatalogBrands.
        /// </summary>
        public DbSet<CataLogBrand> CataLogBrands { get; set; }
        
        /// <summary>
        /// Constructor initalization.
        /// </summary>
        /// <param name="dbContextOptions">Dbcontext options.Connection string passed via dncontext options.</param>
        public CatalogDBContext(DbContextOptions<CatalogDBContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CatalogBrandEntityCOnfiguration());
            modelBuilder.ApplyConfiguration(new CatalogItemEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogTypeEntityConfiguration());
        }
    }
}
