using KamilCataLogAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KamilCataLogAPI.Repository.Interface
{
    /// <summary>
    /// Interface for CatalogRepo.
    /// </summary>
   public interface ICatalogRepo
    {
        /// <summary>
        /// GetAllCatalogItems.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CatalogItem>> GetAllCatalogItems();

        Task<CatalogItem> GetCatalogItem(int Id);

        IQueryable<CatalogItem> GetCataLogItemsByBrandType(int brandTypeId);


        /// <summary>
        /// Get CatalogItems by Id.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>internally this will return object of IQueryable.</returns>
        IEnumerable<CatalogItem> GetCatalogItemsById(string ids);

        Task<IEnumerable<CatalogItem>> GetCatalogItemsByPaging( int pageSize, int pageIndex);

        Task<CatalogItem> GetTopCatalogItemAsync();

        Task<int> UpdateTopCatalogItem(CatalogItem catalogItem);
    }
}
