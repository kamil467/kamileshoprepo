using KamilCataLogAPI.DBContext;
using KamilCataLogAPI.Model;
using KamilCataLogAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KamilCataLogAPI.Repository.Implementation
{
    public class CatalogRepo : ICatalogRepo
    {
        private CatalogDBContext _dbContext;

        /// <summary>
        /// constructor initailzation.
        /// </summary>
        /// <param name="catalogDBContext">catalogDbContext object.</param>
        public CatalogRepo(CatalogDBContext catalogDBContext)
        {
            this._dbContext = catalogDBContext;
        }

        /// <summary>
        /// Get All CatalogItems from database.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IEnumerable<CatalogItem>> GetAllCatalogItems()
        {
            // We are not applying any filter so IEnumerable is best choice here.
            var result = await this._dbContext.CatalogItems
                         .ToListAsync()
                         .ConfigureAwait(false);               

            return result;
        }

        /// <summary>
        /// Returns Single Catalog Item based on ID.
        /// </summary>
        /// <param name="Id">Catalog ID.</param>
        /// <returns></returns>
        public async Task<CatalogItem> GetCatalogItem(int Id)
        {
            return await this._dbContext.CatalogItems
                             .Where(c => c.Id == Id)
                             .SingleOrDefaultAsync()
                             .ConfigureAwait(false);
        }
        
        /// <summary>
        /// Returns IQueryable of catalogItems by brandType ID.
        /// </summary>
        /// <param name="brandTypeId">brand type id.</param>
        /// <returns>IQueryable result.</returns>
        public  IQueryable<CatalogItem> GetCataLogItemsByBrandType(int brandTypeId)
        {
            return   this._dbContext
                              .CatalogItems.Where(c => c.CatalogBrandId == brandTypeId);

        }

        /// <summary>
        /// It returns IQuerbale object. This is diferred execution.
        /// The result can be modified later and final query with all filters will be sent to DB server.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public IEnumerable<CatalogItem> GetCatalogItemsById(string ids)
        {                             

            if (ids == null)
                throw new NullReferenceException("Item id cannot be null.");

            var list = ids.Split(',').Select(id => new
                       {
                        Ok = int.TryParse(ids, out int x),
                        Value = x,
                       });

            // another approach 
            // IEnumerable<bool result, int value)  without using new keyword.
            var list1 = ids.Split(',').Select(s => (result: int.TryParse(s, out int x), value: x));


            if(list.All(l => l.Ok == false))
            {
                // IDS are not integers.
                // log message here
                return new List<CatalogItem>();
            }

            var listOfIds = list.Where(l => l.Ok == true).Select(s => s.Value);

            //result is object of IQueryable.
            var result =  this._dbContext.CatalogItems.
                               Where(c => c.Id == 100)
                               .Select(s=> s);

            
            return result; // at this point IQueryProvider translate the query
                                               // into sql command and send it to DB server for execution
      // we are returning IQueryable since it is acceptable it inherits from IEnumerable.
        }

  
    }
}
