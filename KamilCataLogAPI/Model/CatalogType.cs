using System.Collections.Generic;

namespace KamilCataLogAPI.Model
{
    /// <summary>
    /// CataLogType Class.
    /// </summary>
    public class CatalogType
    {
        /// <summary>
        /// Get or Set ID.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Get or Set Type;
        /// </summary>
        public virtual string Name { get; set; }

        #region Navigational Property
        public virtual ICollection<CatalogItem> CatalogItems { get; set; }
        #endregion
    }
}
