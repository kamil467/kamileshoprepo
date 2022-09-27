using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace KamilCataLogAPI.Extensions
{
    public static class SwaggerWithAPIVersionSupport
    {
        public static void AddCustomSwaggerWithAPIVersionSupport(this IServiceCollection services)
        {
           
            services.AddSwaggerGen(options =>
            {

                var provider = services.BuildServiceProvider()
                                       .GetRequiredService<IApiVersionDescriptionProvider>();

                options.DescribeAllParametersInCamelCase();


          

                var specificationForAPI_Two = new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Kamil-EshopContainers- Catalog HTTP API-V2",
                    Version = "v2",
                    Description = "Future Version(V2) for KamilShop Catalog Microservice API "
                 ,
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Email = "kamil.hussain@qburst.com",
                        Name = "Kamil.Hussain",
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "Kamil.Hussain"
                    },
                };

                //Add swagger doc for each discovered API Version
                // here we make use of APIExplorer , this will discover all available API versioning in current system.
                // details can be extracted from IApiVersionDescriptionProvider.
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateOpenAPIInfoForDescription(description));
                }

            });

     
        }

        public static OpenApiInfo CreateOpenAPIInfoForDescription(ApiVersionDescription apiVersionDescription)
        {
            if (apiVersionDescription == null)
                throw new NullReferenceException();

            var specificationForAPI_One = new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Kamil-EshopContainers- Catalog HTTP API-V:"+apiVersionDescription.GroupName,
                Version = "v"+apiVersionDescription.ApiVersion.MajorVersion,
                Description = "The Catalog Microservice HTTP API. " +
              "This is a data driven/CRUD microservice study application"
             ,
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Email = "kamil.hussain@qburst.com",
                    Name = "Kamil.Hussain",
                },
                License = new Microsoft.OpenApi.Models.OpenApiLicense
                {
                    Name = "Kamil.Hussain"
                },
            };

            return specificationForAPI_One;
        }
    }
}
