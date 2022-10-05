using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace KamilCataLogAPI.Model.Configurations
{
    public class MessageQueueConfiguration
    {
        public const string MessageQueueConfigurationSection = "MessageQueueConfiguration";

        [JsonProperty("Limit")]
        [Range(100,1000,ErrorMessage ="Limit should be between 100 and 1000.")]
       public int Limit { get; set; }
    }
}
