using RLauncher.Yaml.Internal;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RLauncher.Json
{
    public static class RLauncherExtensions
    {
        public static void LoadFromJson(this Launcher launcher, string jsonText)
        {
            if (launcher is null)
                throw new ArgumentNullException(nameof(launcher));

            launcher.LoadFrom(JsonUtils.ToLauncherData(jsonText));
        }

        public static void LoadFromJson(this Runner runner, string jsonText)
        {
            if (runner is null)
                throw new ArgumentNullException(nameof(runner));
            if (jsonText is null)
                throw new ArgumentNullException(nameof(jsonText));

            var obj = JsonSerializer.Deserialize<RunnerJson>(jsonText);
            runner.LoadFrom(obj);
        }
    }
}
