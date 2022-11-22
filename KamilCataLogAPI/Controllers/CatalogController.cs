using CacheHelper.Operations;
using KamilCataLogAPI.Model;
using KamilCataLogAPI.Model.Configurations;
using KamilCataLogAPI.Repository.Interface;
using KamilCataLogAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pipelines.Sockets.Unofficial.Buffers;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private ICatalogRepo _catalogRepo;

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

        


        public CatalogController(ICatalogRepo catalogRepo,
            IOptions<CatalogAPISetting> options,
            IOptionsSnapshot<CatalogAPISetting> optionsSnapshot,
            IOptionsMonitor<CatalogAPISetting> optionsMonitor,
            IOptionsSnapshot<CatalogAPISwaggerConfigurationOptions> swaggeroCnfig,
            IOptionsSnapshot<MessageQueueConfiguration> messageQueueConfiguration
            )
        {
              this._catalogRepo = catalogRepo;
            this.CatalogAPISetting = options.Value; // Standard singleton services
            this.CatalogAPISetting_SnapShot = optionsSnapshot.Value;
            this.CatalogAPISetting_Monitor = optionsMonitor;
            this.CatalogAPISetting_Monitor.OnChange(this.OnChange);
            this._swaggerAPIV1 = swaggeroCnfig.Get(CatalogAPISwaggerConfigurationOptions.V1); // Named options
            this._swaggerAPIV2 = swaggeroCnfig.Get(CatalogAPISwaggerConfigurationOptions.V2);  // Named Options
            this._messageQueueConfiguration = messageQueueConfiguration.Value;
        }

        /// <summary>
        /// This will get called whenever a value change occurred in Configuration setting.
        /// </summary>
        /// <param name="obj"></param>
        private void OnChange(object obj)
        {
            // your logic goes here.
        }


        [HttpGet("GetCataLogItems/{ids}")]
        public IEnumerable<CatalogItem> GetCataLogItems(string ids = null)
        {
           var result =  this._catalogRepo.GetCatalogItemsById(ids).ToList(); // deferred execution happen here before sending response.
            return result;
        }



        // GET: api/<CatalogController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CatalogController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CatalogController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CatalogController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CatalogController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //this one will be served from v1 while remaining endpoints served from v2 for this controller.
        /// api/v1/catalog/GetNumericData
        [MapToApiVersion("1.0")]
        [HttpGet("GetNumericData")]
        /// <summary>
        /// returns numeric data.
        /// </summary>
        /// <returns>1</returns>
        public int GetNumericData()
        {
            return 1;
        }

        /// <summary>
        /// Returns paginated data.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCataLogItemsByPagination")]
        [ProducesResponseType((int)HttpStatusCode.OK,Type = typeof(IEnumerable<CatalogItem>))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedItemsViewModel<CatalogItem>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCataLogItems(
            [FromQuery] int pageSize =10, [FromQuery] int pageIndex = 0,string ids =null
            )
        {
            // ids passed
            if(!string.IsNullOrEmpty(ids))
            {
                var dataById = this._catalogRepo.GetCatalogItemsById(ids);
                
                // if no data for given IDS
                if(!dataById.Any())
                {
                    return BadRequest("Please provide valid IDS");
                }

                return Ok(dataById);
            }

            // no ids passed then take pagination.

            var catData = await this._catalogRepo.GetCatalogItemsByPaging(pageSize, pageIndex)
                        .ConfigureAwait(false);

            var pageModel = new PaginatedItemsViewModel<CatalogItem>
            {
                Items = catData,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return Ok(pageModel);
        }

        [HttpGet]
        [Route("GetTopItem")]
        [ProducesResponseType((int)HttpStatusCode.OK,Type =typeof(CatalogItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTopCatalogItem()
        {
            //Implemented Read-through 

            var topCatalogItem = await this._catalogRepo
                                 .GetTopCatalogItemAsync()
                                 .ConfigureAwait(false);
            if(topCatalogItem == null)
                return NotFound();

            return Ok(topCatalogItem);
        }

        /// <summary>
        /// Put Request use NoContent (204) to send blank response.
        /// </summary>
        /// <param name="catalogItem"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateTopCatalogItem")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTopCataLogItem([FromBody] CatalogItem catalogItem)
        {
            // Implemented Write-Through Pattern

            if(catalogItem == null)
            {
                return this.BadRequest();
            }

            var result = await this._catalogRepo
                       .UpdateTopCatalogItem(catalogItem).ConfigureAwait(false);
         
            // return -404 if no update performed.
            if (result == 0)
                return NotFound();

            return NoContent();
        }
    }
}
