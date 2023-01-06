using KamilCataLogAPI.Model;
using KamilCataLogAPI.Model.DTO;
using System.Linq;

namespace KamilCataLogAPI.QueryObjects
{
    
    public static class CatalogItemFilterByBrand
    {
        public static IQueryable<CatalogItemDTO> CatalogItemsByBrand(this IQueryable<CatalogItem> catalogItems, int brandId)
        {
            return catalogItems.Where(a => a.CataLogBrand.Id == brandId).Select(catItem => new CatalogItemDTO
            {
                Title = catItem.Name,
                Brand = catItem.CataLogBrand.Name,
                Description = catItem.Description,
                AvailableStock = catItem.AvailableStock.HasValue?catItem.AvailableStock.Value:0,
                Price = catItem.Price.HasValue?catItem.Price.Value:0,
                PictureUri = catItem.PictureUri,
                Type = catItem.CatalogType.Name
            });
        }
    }
}
