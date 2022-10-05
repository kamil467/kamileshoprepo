using KamilCataLogAPI.Model.Configurations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace KamilCataLogAPI.Extensions
{
    public static class SwaggerWithAPIVersionSupport
    {
        public static void AddCustomSwaggerWithAPIVersionSupport(this IServiceCollection services, IConfiguration configuration)
        {
            //load swagger meta data details from configuration.

            // Get() - > internally binds and return the specified type.
            var v1MetaData = configuration
                           .GetSection(CatalogAPISwaggerConfigurationOptions.Swagger)
                           .GetSection(CatalogAPISwaggerConfigurationOptions.V1)
                           .Get<CatalogAPISwaggerConfigurationOptions>();
            
            // load for V2 version
            var v2MetaData = configuration
                          .GetSection(CatalogAPISwaggerConfigurationOptions.Swagger)
                          .GetSection(CatalogAPISwaggerConfigurationOptions.V2)
                          .Get<CatalogAPISwaggerConfigurationOptions>();


            // v1metadata and v2metadata can be converted and registered as Named Options.
            #region Named Opions
           
            services.Configure<CatalogAPISwaggerConfigurationOptions>(CatalogAPISwaggerConfigurationOptions.V1,configuration
                      .GetSection(CatalogAPISwaggerConfigurationOptions.Swagger)
                      .GetSection(CatalogAPISwaggerConfigurationOptions.V1));

            services.Configure<CatalogAPISwaggerConfigurationOptions>(CatalogAPISwaggerConfigurationOptions.V2,configuration
                      .GetSection(CatalogAPISwaggerConfigurationOptions.Swagger)
                      .GetSection(CatalogAPISwaggerConfigurationOptions.V2));

            #endregion
            services.AddSwaggerGen(options =>
            {

                var provider = services.BuildServiceProvider()
                                       .GetRequiredService<IApiVersionDescriptionProvider>();

                options.DescribeAllParametersInCamelCase();

                //Add swagger doc for each discovered API Version
                // here we make use of APIExplorer , this will discover all available API versioning in current system.
                // details can be extracted from IApiVersionDescriptionProvider.
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    if (description.GroupName == "v1")
                        options.SwaggerDoc(description.GroupName, CreateOpenAPIInfoForDescription(description, v1MetaData));
                    else if (description.GroupName == "v2")
                        options.SwaggerDoc(description.GroupName, CreateOpenAPIInfoForDescription(description, v2MetaData));
                    else
                        options.SwaggerDoc(description.GroupName, CreateOpenAPIInfoForDescription(description, null)); 
                }

            });

     
        }

        public static OpenApiInfo CreateOpenAPIInfoForDescription(ApiVersionDescription apiVersionDescription, CatalogAPISwaggerConfigurationOptions catalogAPISwaggerConfigurationOptions )
        {
            if (apiVersionDescription == null)
                throw new NullReferenceException();

            if(catalogAPISwaggerConfigurationOptions == null)
            {
                // print log
            }

            var specificationForAPI_One = new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = catalogAPISwaggerConfigurationOptions?.Title,
                Version = "v"+apiVersionDescription.ApiVersion.MajorVersion,
                Description = catalogAPISwaggerConfigurationOptions?.Description,
              
             
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Email = catalogAPISwaggerConfigurationOptions?.Contact?.Email,
                    Name = catalogAPISwaggerConfigurationOptions?.Contact?.Name,
                },
                License = new Microsoft.OpenApi.Models.OpenApiLicense
                {
                    Name = catalogAPISwaggerConfigurationOptions?.Contact?.Name
                },
            };

            return specificationForAPI_One;
        }
    }
}
