
using YamlDotNet.Serialization;

namespace RLauncher.Yaml.Internal
{
    class RunnerYaml : IRunnerData
    {
        [YamlMember(Alias = PropertyNames.Executable, ApplyNamingConventions = false)]
        public string? Executable { get; set; }

        [YamlMember(Alias = PropertyNames.Arguments, ApplyNamingConventions = false)]
        public string?[]? Arguments { get; set; }

        [YamlMember(Alias = PropertyNames.WorkingDirectory, ApplyNamingConventions = false)]
        public string? WorkingDirectory { get; set; }
    }
}
