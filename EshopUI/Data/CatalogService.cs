using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;

namespace EshopUI.Data
{
    public class CatalogService
    {
        private readonly HttpClient _httpClient;

        private readonly IOptions<CatalogAPISettings> _settings;

        public CatalogService(HttpClient httpClient,
            IOptions<CatalogAPISettings> options)
        {
            this._httpClient = httpClient;
            this._settings = options;
        }
        public async Task<CatalogItem[]> GetCatalogItemsASync()
        {
            var uriBuilder = new Uri($"{this._settings.Value.APIUrl}/{this._settings.Value.GetCataLogItem}");

            var response = await this._httpClient
                          .GetAsync(uriBuilder)
                          .ConfigureAwait(false);
           
            if(!response.IsSuccessStatusCode)
            {
                throw new ApplicationException("http call was not successfull");
            }
        
              var content = await response.Content.ReadAsStringAsync()
                         .ConfigureAwait(false);
            var catalogItems = JsonConvert.DeserializeObject<CatalogItemPaginated<CatalogItem>>(content);

            

            return catalogItems.Items.ToArray();
        }
    }
}
