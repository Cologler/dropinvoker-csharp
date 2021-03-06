﻿using System.Text.Json.Serialization;

namespace DropInvoker.Models.Configurations
{
    class LauncherJson
    {
        [JsonPropertyName("app")]
        public string? Application { get; set; }

        [JsonPropertyName("args")]
        public string?[]? Args { get; set; }

        [JsonPropertyName("workdir")]
        public string? WorkDir { get; set; }
    }
}
