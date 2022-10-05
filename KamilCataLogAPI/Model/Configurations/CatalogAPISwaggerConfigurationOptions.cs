using Newtonsoft.Json;

namespace KamilCataLogAPI.Model.Configurations
{
    public class CatalogAPISwaggerConfigurationOptions
    {

        /// <summary>
        /// Setting Configuration Section Name.
        /// </summary>
        public const string Swagger = "Swagger";

        /// <summary>
        /// V1
        /// </summary>
        public const string V1 = "V1";

        /// <summary>
        /// V2
        /// </summary>
        public const string V2 = "V2";

        /// <summary>
        /// Get or Set Title.
        /// </summary>
        [JsonProperty("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Get or Set Description
        /// </summary>
        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Contact")]
        public SwaggerContact Contact { get; set; }
    }


    public class SwaggerContact
    {

        /// <summary>
        /// Get or Set Name.
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or Set Email
        /// </summary>
        [JsonProperty("Email")]
        public string Email { get; set; }
    }
}
