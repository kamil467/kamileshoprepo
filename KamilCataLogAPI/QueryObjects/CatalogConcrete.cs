using KamilCataLogAPI.DBContext;
using KamilCataLogAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KamilCataLogAPI.QueryObjects
{
    public class CatalogConcrete : ICatalogService
    {
        private readonly CatalogDBContext catalogDBContext;

        public CatalogConcrete(CatalogDBContext catalogDBContext)
        {
            this.catalogDBContext = catalogDBContext;
        }
        public IEnumerable<CatalogItemDTO> GetCatalogItemsByBrand(int brandId)
        {
            return this.catalogDBContext.CatalogItems
                    .Include(x => x.CataLogBrand)
                    .Include(x => x.CatalogType)
                    .CatalogItemsByBrand(brandId);
                   
        }
    }
}
