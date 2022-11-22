using CacheHelper.Operations;
using KamilCataLogAPI.Model;
using KamilCataLogAPI.Model.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace KamilCataLogAPI.Extensions
{
    public static class RedisConfiguration
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            // we are not registering into services.
            // we just only bind the configuration to .NET objects.
            var redisConfig = configuration.GetSection(RedisSettings.RedisConfigurationSection)
                                            .Get<RedisSettings>();
            var multiplexer = ConnectionMultiplexer.Connect(redisConfig.Server+","+"password="+redisConfig.Password);

            //we register as a Singleton so it is always maintains only one instance throughout lifetime of application.
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            ///register redisbasehelper - Factory class for providing instance for all type of database.
            services.AddSingleton<IRedisBaseHelper<string,CatalogItem>, RedisBaseHelper<string,CatalogItem>>();

        }
    }
}
