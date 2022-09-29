using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
    }
}
