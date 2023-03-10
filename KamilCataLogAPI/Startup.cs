using CacheHelper.Operations;
using KamilCataLogAPI.Extensions;
using KamilCataLogAPI.QueryObjects;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KamilCataLogAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCustomAPIVersion(); // enable custom API versioning.
           
            //Add and configure swagger service.
            services.AddCustomSwaggerWithAPIVersionSupport(this.Configuration);

            // Add custom DB configuration
            services.AddDBConfiguration(this.Configuration);

            // uses Option Builder and Services.Configure approaches
            services.AddCatalogAPISettingConfiguration(this.Configuration);

            services.AddTransient<ICatalogService, CatalogConcrete>();
            
            //Identity Server Configuration
            // services.AddIdentityServerConfiguration(); enable when required
           
            //register redis connection multiplexer as a signleton instance
          //  services.AddRedis(this.Configuration); -- commenting this is for development purpose. enable when required

          

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //Enable Swagger in the middleware
            app.UseSwagger().UseSwaggerUI(c =>
            {

               foreach(var versionProvider in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{versionProvider.GroupName}/swagger.json", $"{versionProvider.GroupName.ToUpperInvariant()}");
                    //  // Swagger metadata json url : JSON gets generated automatically every we visit this url.
                }
            });
            app.UseAuthentication();
           
            app.UseRouting();  // this line not required in .NET 6

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
