using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KamilCataLogAPI.Controllers
{
    /// <summary>
    /// API Controller class.
    /// </summary>
    // [ApiVersion("1")] // this moved global api version handler
    [ApiController]
    [Route("api/Catalog")] // default route url - this will not work since without version will not allow.- update the setting in global api versioning 
    //test url : api/v1/Catalog
    [Route("api/v{version:apiVersion}/Catalog")] // API version route:working route
    public class CatalogOldController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CatalogOldController> _logger;

        public CatalogOldController(ILogger<CatalogOldController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
