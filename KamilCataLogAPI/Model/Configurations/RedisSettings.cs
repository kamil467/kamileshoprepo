using Newtonsoft.Json;
using System.Security.Policy;

namespace KamilCataLogAPI.Model.Configurations
{
    public class RedisSettings
    {
        public const string RedisConfigurationSection = "RedisConfiguration";

        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
