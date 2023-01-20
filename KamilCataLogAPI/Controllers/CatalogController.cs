using CacheHelper.Operations;
using KamilCataLogAPI.Model;
using KamilCataLogAPI.Model.Configurations;
using KamilCataLogAPI.Model.DTO;
using KamilCataLogAPI.QueryObjects;

using KamilCataLogAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pipelines.Sockets.Unofficial.Buffers;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KamilCataLogAPI.Controllers
{
    //[ApiVersion("2.0")] // this moved global api version handler
    [Route("api/[controller]")] 
    //test url : api/v1/Catalog
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private ICatalogService catalogService;

        private readonly ILogger<CatalogController> _logger; 
        /// <summary>
        /// IOption<CatalogAPISetting> Singleton Instance.
        /// </summary>
        private CatalogAPISetting CatalogAPISetting;

        /// <summary>
        /// snapshot variant - read updated  value every time.
        /// </summary>
        private CatalogAPISetting CatalogAPISetting_SnapShot;

        /// <summary>
        /// Monitor variant.
        /// </summary>
        private IOptionsMonitor<CatalogAPISetting> CatalogAPISetting_Monitor;

        private CatalogAPISwaggerConfigurationOptions _swaggerAPIV1;

        private CatalogAPISwaggerConfigurationOptions _swaggerAPIV2;

        private MessageQueueConfiguration _messageQueueConfiguration;

        


        public CatalogController(
            IOptions<CatalogAPISetting> options,
            IOptionsSnapshot<CatalogAPISetting> optionsSnapshot,
            IOptionsMonitor<CatalogAPISetting> optionsMonitor,
            IOptionsSnapshot<CatalogAPISwaggerConfigurationOptions> swaggeroCnfig,
            IOptionsSnapshot<MessageQueueConfiguration> messageQueueConfiguration,
            ICatalogService catalogService,
            ILogger<CatalogController> logger
            )
        {
            this.catalogService = catalogService;
            this.CatalogAPISetting = options.Value; // Standard singleton services
            this.CatalogAPISetting_SnapShot = optionsSnapshot.Value;
            this.CatalogAPISetting_Monitor = optionsMonitor;
            this.CatalogAPISetting_Monitor.OnChange(this.OnChange);
            this._swaggerAPIV1 = swaggeroCnfig.Get(CatalogAPISwaggerConfigurationOptions.V1); // Named options
            this._swaggerAPIV2 = swaggeroCnfig.Get(CatalogAPISwaggerConfigurationOptions.V2);  // Named Options
            this._messageQueueConfiguration = messageQueueConfiguration.Value;
            this._logger = logger;
        }

        /// <summary>
        /// This will get called whenever a value change occurred in Configuration setting.
        /// </summary>
        /// <param name="obj"></param>
        private void OnChange(object obj)
        {
            // your logic goes here.
        }

        [HttpGet]
        [Route("")]
        public string Index()
        {
            return "Catalo]gAPI";
        }

        [HttpGet]
        [Route("GetCatalogItemByBrand")]
        [ProducesResponseType(typeof(IEnumerable<CatalogItemDTO>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult<IEnumerable<CatalogItemDTO>> GetCatalogItemsByBrand(int brandId)
        {
            if (brandId == 0)
            {
                _logger.LogInformation($"Incorrect brand Id thrown:{brandId}");
                return BadRequest();
            }
            var result = this.catalogService.GetCatalogItemsByBrand(brandId).ToList();
            if (!result.Any())
                return NotFound();

            return Ok(result);
        }
    }
}
