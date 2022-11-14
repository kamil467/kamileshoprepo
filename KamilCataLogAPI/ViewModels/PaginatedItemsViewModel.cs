using System.Collections.Generic;

namespace KamilCataLogAPI.ViewModels
{
    public class PaginatedItemsViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
