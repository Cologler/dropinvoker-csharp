using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DropInvoker.Models.Configurations
{
    class SceneJson
    {
        [JsonPropertyName("slots")]
        public string?[]? Slots { get; set; }
    }
}
