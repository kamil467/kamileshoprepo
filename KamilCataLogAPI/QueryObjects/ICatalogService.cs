using KamilCataLogAPI.Model.DTO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KamilCataLogAPI.QueryObjects
{
    public interface ICatalogService
    {
        IEnumerable<CatalogItemDTO> GetCatalogItemsByBrand(int brandId);
    }
}
