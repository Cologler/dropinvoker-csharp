
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RLauncher.Json.Internal
{
    class LauncherJson : ILauncherData
    {
        [JsonPropertyName(PropertyNames.Name)]
        public string? Name { get; set; }

        [JsonPropertyName(PropertyNames.Description)]
        public string? Description { get; set; }

        [JsonPropertyName(PropertyNames.Runner)]
        public string? Runner { get; set; }

        [JsonPropertyName(PropertyNames.Arguments)]
        public string?[]? Arguments { get; set; }

        [JsonPropertyName(PropertyNames.WorkingDirectory)]
        public string? WorkingDirectory { get; set; }

        [JsonPropertyName(PropertyNames.Accepts)]
        public string?[]? Accepts { get; set; }
    }
}
