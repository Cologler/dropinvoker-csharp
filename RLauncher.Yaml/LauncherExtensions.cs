using RLauncher.Yaml.Internal;

using System;
using System.Collections.Generic;
using System.IO;

using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace RLauncher.Yaml
{
    public static class LauncherExtensions
    {
        public static void LoadFromYaml(this Launcher launcher, TextReader reader)
        {
            if (launcher is null)
                throw new ArgumentNullException(nameof(launcher));
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var deserializer = new Deserializer();
            var obj = deserializer.Deserialize<LauncherYaml>(reader);
            launcher.LoadFrom(obj);
        }
    }
}
