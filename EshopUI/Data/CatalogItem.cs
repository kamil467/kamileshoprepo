

namespace EshopUI.Data
{
    public class CatalogItem
    {
        /// <summary>
        /// Get or Set Id.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Get or Set Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Get or Set Description.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Get or Set Price.
        /// </summary>
        public virtual decimal? Price { get; set; }

        /// <summary>
        /// Get or set CatalogType Id.
        /// </summary>
        public virtual int? CatalogTypeId { get; set; }

        /// <summary>
        /// Get or Set CataLogType.
        /// </summary>
      //  public virtual CatalogType CatalogType { get; set; }

        /// <summary>
        /// Get or Set CatalogBrandId.
        /// </summary>
        public virtual int? CatalogBrandId { get; set; }
    }
}
