using Newtonsoft.Json;

namespace KamilCataLogAPI.Model.Configurations
{
    public class CatalogAPISetting
    {
        public const string CatalogAPISettingSection = "CatalogAPISetting";

        [JsonProperty("UserId")]
        public int UserId { get; set; }
    }
}
