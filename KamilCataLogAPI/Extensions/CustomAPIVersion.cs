using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using KamilCataLogAPI.Controllers;

namespace KamilCataLogAPI.Extensions
{
    /// <summary>
    /// CustomAPIVersion class.
    /// </summary>
    public static class CustomAPIVersion
    {
        /// <summary>
        /// Adding custome API version extension method of Iservice interface.
        /// </summary>
        /// <param name="services">service collection objects.</param>
        public static void AddCustomAPIVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                //add information to response headers(api-supported-version,api-deprecated-version)
                // which version is supported and which going to deprecate
                o.ReportApiVersions = true; // in our case we will write to response as api-supported-version:1.0 
              
                //our API shoule be backward compatability even if customer not mentioning API version number. 
                o.AssumeDefaultVersionWhenUnspecified = false;
                // false - it will block all the urls without version.( this will block even separate route defined) 
                // true - this will allow uirls without version - but not working - we have to define route for it. THIS IS A BUG.


               // o.DefaultApiVersion = new ApiVersion(1, 0); // this is the first versioning of our API. No need to specify at controller
                                                            // if we set here.
                                                            // It is a good idea to set AssumDefaultVersionWhenUnSpecified = false
                                                            // and define default route for Headertypes sources and url version  route.
                                                            // We are having two versions of same controller hence adding the version number to each controller will be the good choice and commenting this line.

                o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"), // reading from query string
                    new HeaderApiVersionReader("Accept-Version"), // reading from header
                    new MediaTypeApiVersionReader("api-version"), // reading from mediatype
                    new UrlSegmentApiVersionReader()  // reading from url
                    );

                o.Conventions.Controller<CatalogOldController>()
                .HasDeprecatedApiVersion(new ApiVersion(1, 0)); // Advertise this as a deprecated api version

                o.Conventions.Controller<CatalogController>()
                             .HasApiVersion(new ApiVersion(2, 0));
               
            });

            //Add support for Versioned APi Explorer - this explore all possible API version in application.
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // format for v'major[.minor][-status]

                options.SubstituteApiVersionInUrl = true; // requires url segment based API versioning
            });

        }
    }
}
