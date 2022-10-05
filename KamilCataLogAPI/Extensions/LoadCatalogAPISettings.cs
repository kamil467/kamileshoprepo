using KamilCataLogAPI.Model.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KamilCataLogAPI.Extensions
{
    public static class LoadCatalogAPISettings
    {
        /// <summary>
        /// Implemented IOption pattern by registering configuration object in service conatiner with Configure<![CDATA[>]]>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCatalogAPISettingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // another approach. uses OptionBuilder API
            // validate , serialize and register
            // AddOptions - inmplicilty calls Configuration and register the option service.
            // If we do not use AddOption then we need to explicility call Services.Configure for registering in DI.
            services.AddOptions<MessageQueueConfiguration>()
                .Bind(configuration.GetSection(MessageQueueConfiguration.MessageQueueConfigurationSection))
                .ValidateDataAnnotations();
               
                
                 
                

            /**--------DI registerring----------**//********************Reading as .NET strongly typed object***********/  
            services.Configure<CatalogAPISetting>(configuration.GetSection(CatalogAPISetting.CatalogAPISettingSection));
            
            
        }
    }
}
