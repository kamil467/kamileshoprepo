using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EshopUI
{
    public class CatalogAPISettings
    {
        public const string CatalogAPISection = "CatalogAPI";

        [ConfigurationKeyName("APIurl")]
        public string? APIUrl { get; set; }

        [ConfigurationKeyName("GetCatalogItems")]
        public string? GetCataLogItem { get; set; }

        [ConfigurationKeyName("GetTopItem")]
        public string? GetTopItem { get; set; }
    }
}
