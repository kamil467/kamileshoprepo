namespace KamilCataLogAPI.Model.DTO
{
    public class CatalogItemDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public int AvailableStock { get; set; }

        public string PictureUri { get; set; }
    }
}
