using KamilCataLogAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KamilCataLogAPI.Extensions
{
    public static class DatabaseConfiguration
    {
        public static void AddDBConfiguration(this IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                     .AddDbContext<CatalogDBContext>(options =>
                     {
                         options.UseSqlServer("Server=QB-TP3-2217;Initial Catalog=Test;User Id=sa;Password=kamil467"); 
                     });
        }
    }
}
