namespace EshopUI.Data
{
    public class CatalogItemPaginated<T> where T : class?
    {
        public IEnumerable<T>? Items { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
