using KamilCataLogAPI.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KamilCataLogAPI.DBContext
{
    /// <summary>
    /// CatalogDBContextSeed for seeding the database when service started for first time.
    /// </summary>
    public  class CatalogDBContextSeed
    {
        public static async Task SeedAsync(IApplicationBuilder applicationBuilder)
        {
            //get db context through application service
            var dbContext = (CatalogDBContext) applicationBuilder
                         .ApplicationServices.GetService(typeof(CatalogDBContext));
            using(dbContext)
            {

                dbContext.Database.Migrate(); // apply migration . create db if not exists.
                
                if(!dbContext.CatalogItems.Any())
                {
                    // seed if data is empty.
                    dbContext.CatalogItems.AddRange(GetCatalogItems());
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);

                }
            }
        }

        private static IEnumerable<CatalogItem> GetCatalogItems()
        {
            return new List<CatalogItem>
            {
                new CatalogItem
                {
                    Id= 100,
                    Name="AAA",
                    Description="Test",
                    Price=10000,
                },
                 new CatalogItem
                {
                    Id= 101,
                    Name="BBB",
                    Description="Test",
                    Price=10000,
                },
                  new CatalogItem
                {
                    Id= 102,
                    Name="CDC",
                    Description="Test",
                    Price=10000,
                },
                   new CatalogItem
                {
                    Id= 103,
                    Name="DDD",
                    Description="Test",
                    Price=10000,
                }
            };
        }
    }
}
