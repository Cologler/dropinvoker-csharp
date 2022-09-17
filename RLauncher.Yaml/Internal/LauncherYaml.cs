using System;
using System.Collections.Generic;
using System.Text;

using RLauncher.Abstractions;

using YamlDotNet.Serialization;

namespace RLauncher.Yaml.Internal
{
    class LauncherYaml : ILauncherData
    {
        [YamlMember(Alias = PropertyNames.Name, ApplyNamingConventions = false)]
        public string? Name { get; set; }

        [YamlMember(Alias = PropertyNames.Description, ApplyNamingConventions = false)]
        public string? Description { get; set; }

        [YamlMember(Alias = PropertyNames.Runner, ApplyNamingConventions = false)]
        public string? Runner { get; set; }

        [YamlMember(Alias = PropertyNames.Arguments, ApplyNamingConventions = false)]
        public string?[]? Arguments { get; set; }

        [YamlMember(Alias = PropertyNames.WorkingDirectory, ApplyNamingConventions = false)]
        public string? WorkingDirectory { get; set; }

        [YamlMember(Alias = PropertyNames.Accepts, ApplyNamingConventions = false)]
        public string?[]? Accepts { get; set; }
    }
}
