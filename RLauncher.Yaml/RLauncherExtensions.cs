using RLauncher.Yaml.Internal;

using System;
using System.Collections.Generic;
using System.IO;

using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace RLauncher.Yaml
{
    public static class RLauncherExtensions
    {
        public static void LoadFromYaml(this Launcher launcher, string yamlText)
        {
            if (launcher is null)
                throw new ArgumentNullException(nameof(launcher));

            launcher.LoadFrom(YamlUtils.ToLauncherData(yamlText));
        }

        public static void LoadFromYaml(this Runner runner, string yamlText)
        {
            if (runner is null)
                throw new ArgumentNullException(nameof(runner));
            if (yamlText is null)
                throw new ArgumentNullException(nameof(yamlText));

            var obj = new Deserializer().Deserialize<RunnerYaml>(yamlText);
            runner.LoadFrom(obj);
        }
    }
}
