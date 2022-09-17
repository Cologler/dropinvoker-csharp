using RLauncher.Abstractions;
using RLauncher.Yaml.Internal;

using System;

using YamlDotNet.Serialization;

namespace RLauncher.Yaml
{
    public static class YamlUtils
    {
        public static ILauncherData ToLauncherData(string yamlText)
        {
            if (yamlText is null)
                throw new ArgumentNullException(nameof(yamlText));
            return new Deserializer().Deserialize<LauncherYaml>(yamlText);
        }
    }
}
