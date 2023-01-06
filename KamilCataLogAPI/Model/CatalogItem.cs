using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KamilCataLogAPI.Model
{
    /// <summary>
    /// CataLogItem class
    /// </summary>
    public class CatalogItem
    {
        /// <summary>
        /// Get or Set Id.
        /// </summary>
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
        public virtual Nullable<decimal> Price { get; set; }


        public virtual string PictureFileName { get; set; }

        public virtual string PictureUri { get; set; }

        public virtual Nullable<int> AvailableStock { get; set; }

        public virtual Nullable<int> RestockThreshold { get; set; }
        
        public virtual Nullable<int> MaxStockThreshold { get; set; }

        public virtual bool OnReorder { get; set; }

        public virtual Nullable<int> BrandId { get; set; }

        public virtual Nullable<int> TypeId { get; set; }

        #region Navigational Properties
        public CataLogBrand CataLogBrand { get; set; }

        public CatalogType CatalogType { get; set; }
        #endregion

    }
}
