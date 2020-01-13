using System.Text.Json.Serialization;

namespace DropInvoker.Models.Configurations
{
    class InvokerJson
    {
        [JsonPropertyName("app")]
        public string? Application { get; set; }

        [JsonPropertyName("desc")]
        public string? Description { get; set; }

        [JsonPropertyName("args")]
        public string?[]? Args { get; set; }

        [JsonPropertyName("accept")]
        public string?[]? Accept { get; set; }

        [JsonPropertyName("workdir")]
        public string? WorkDir { get; set; }
    }
}
