using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace KamilCataLogAPI.Extensions
{
    public static class MigrationDBContext
    {
        /// <summary>
        /// Helps migration and seeding database.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="host"></param>
        /// <param name="seeder"></param>
        /// <returns></returns>
        public static IHost MigrateDbContext<TContext>(this IHost host,
            Action<TContext,IServiceProvider> seeder                     
            ) where TContext:DbContext
        {
           using(var scope = host.Services.CreateScope())
             {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<TContext>();

                //var retry = Policy.Handle

            }
            return host;
        }
    }
}
