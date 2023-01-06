using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KamilCataLogAPI.Model
{
    /// <summary>
    /// CatalogBrand class.
    /// </summary>
    /// 
    public class CataLogBrand
    {
        /// <summary>
        /// Get or Set Id.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Get or Set Brand.
        /// </summary>
        public virtual string Name { get; set; }
        
        #region Navigational Property
        public virtual ICollection<CatalogItem> CatalogItems { get; set; }
        #endregion

    }
}
