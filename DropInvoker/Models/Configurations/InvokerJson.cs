using RLauncher;

using System.Text.Json.Serialization;

namespace DropInvoker.Models.Configurations
{
    class InvokerJson
    {
        [JsonPropertyName(PropertyNames.Runner)]
        public string? Runner { get; set; }

        [JsonPropertyName("app")]
        public string? Application { get; set; }

        [JsonPropertyName(PropertyNames.Description)]
        public string? Description { get; set; }

        [JsonPropertyName(PropertyNames.Arguments)]
        public string?[]? Args { get; set; }

        [JsonPropertyName(PropertyNames.Accepts)]
        public string?[]? Accept { get; set; }

        [JsonPropertyName(PropertyNames.WorkingDirectory)]
        public string? WorkDir { get; set; }
    }
}
