using System.Text.Json.Serialization;

namespace RLauncher.Yaml.Internal
{
    class RunnerJson : IRunnerData
    {
        [JsonPropertyName(PropertyNames.Executable)]
        public string? Executable { get; set; }

        [JsonPropertyName(PropertyNames.Arguments)]
        public string?[]? Arguments { get; set; }

        [JsonPropertyName(PropertyNames.WorkingDirectory)]
        public string? WorkingDirectory { get; set; }
    }
}
