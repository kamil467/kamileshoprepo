using CacheHelper.Operations;
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
        private readonly CatalogDBContext _dbContext;

        private readonly IRedisHelper<string, CatalogItem> redisZeroDBHelper;
        /// <summary>
        /// constructor initailzation.
        /// </summary>
        /// <param name="catalogDBContext">catalogDbContext object.</param>
        public CatalogRepo(CatalogDBContext catalogDBContext,
            IRedisBaseHelper<string, CatalogItem> redisBase)
        {
            this._dbContext = catalogDBContext;
            this.redisZeroDBHelper = redisBase.GetRedisHelper("catalog", 0); // calling default db
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

        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsByPaging( int pageSize, int pageIndex)
        {

            return await this._dbContext.CatalogItems.OrderBy(o => o.Name)
                      .Skip(pageIndex * pageSize)
                      .Take(pageSize)
                      .ToListAsync();
                          
        }

        /// <summary>
        /// Implements Cache-A Side Pattern
        /// First Look for the item in cache and return the item if found.
        /// Look for db and add the item in cache and return if not found.
        /// </summary>
        /// <returns></returns>
        public async Task<CatalogItem> GetTopCatalogItemAsync()
        {
            #region Redis Lookup

            var key = "top_catalog_item_get";
            var expiration = TimeSpan.FromMinutes(3); // adding 3 minutes
            bool cacheException = false;
            try
            {
                var item = this.redisZeroDBHelper.GetItem(key);
                if(item != null)
                {
                    return item;
                }
            }
            catch(Exception e)
            {
                
                cacheException = true;

                // additional execption handling logic goes here.
            }
            #endregion

            #region Read-Through

            var entity = await this._dbContext.CatalogItems
                         .Select(c => c)
                         .FirstOrDefaultAsync();

            // add item to cache only if cache is healthy and no exeception.
            if(!cacheException)
            {
                try
                {
                    if (entity != null)
                    {
                        // ensure adding non null items

                        this.redisZeroDBHelper.Add(key, entity,expiration);
                    }
                }
                catch(Exception e)
                {
                    cacheException = true;
                }
            }
            
            return entity;
            #endregion
        }

        /// <summary>
        /// Write-Through Logic
        /// First update the item in database.
        /// Second InValidate Cache object.
        /// </summary>
        /// <param name="catalogItem"></param>
        public async Task<int> UpdateTopCatalogItem(CatalogItem catalogItem)
        {
            var key = "top_catalog_item_get";
            var entity = await this._dbContext.CatalogItems
                          .Where(c => c.Id == catalogItem.Id).FirstOrDefaultAsync();
            if (entity == null)
            {

                var redisObj1 = this.redisZeroDBHelper.GetItem(key);

                if (redisObj1 != null)
                    this.redisZeroDBHelper.Remove(key);
                return 0;  // item not found and no update
            }
          
            entity.Description = catalogItem.Description;
            entity.Price= catalogItem.Price;
            entity.CatalogBrandId = catalogItem.CatalogBrandId;
            entity.CatalogTypeId = catalogItem.CatalogTypeId;
            entity.Name = catalogItem.Name;
            
            var result =  await  this._dbContext.SaveChangesAsync();


            var redisObj = this.redisZeroDBHelper.GetItem(key);

            if (redisObj != null)
                this.redisZeroDBHelper.Remove(key);

            return result;

        }
    }
}
