using KamilCataLogAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KamilCataLogAPI.Extensions
{
    public static class DatabaseConfiguration
    {
        public static void AddDBConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                     .AddDbContext<CatalogDBContext>(options =>
                     {
                         //options.UseSqlServer("Server=QB-TP3-2217;Initial Catalog=Test;User Id=sa;Password=kamil467"); 
       
                         // appsettings.json , ConnectionStrings section 
                         options.UseSqlServer(configuration.GetConnectionString(AppConstants.CatalogDB));
                    
       
                     });
        }
    }
}
