using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KamilCataLogAPI.Model
{
    /// <summary>
    /// CataLogItem class
    /// </summary>
    [Table("CatalogItem")]
    public class CatalogItem
    {
        /// <summary>
        /// Get or Set Id.
        /// </summary>
        [Key]
        [Column("Id")]
        public virtual int  Id { get; set; }

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

        /// <summary>
        /// Get or Set CatalogBrand.
        /// </summary>
        // public virtual CataLogBrand CataLogBrand { get; set; }
    }
}
