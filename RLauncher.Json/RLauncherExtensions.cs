using RLauncher.Exceptions;
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

            try
            {
                if (JsonUtils.ToLauncherData(jsonText) is not { } data)
                {
                    throw new InvalidRLauncherConfigurationFileException()
                    {
                        FileType = ConfigurationFileTypes.Launcher,
                        FileContent = jsonText
                    };
                }

                launcher.LoadFrom(data);
            }
            catch (JsonException)
            {
                throw new InvalidRLauncherConfigurationFileException()
                {
                    FileType = ConfigurationFileTypes.Launcher,
                    FileContent = jsonText
                };
            }
        }

        public static void LoadFromJson(this Runner runner, string jsonText)
        {
            if (runner is null)
                throw new ArgumentNullException(nameof(runner));
            if (jsonText is null)
                throw new ArgumentNullException(nameof(jsonText));

            try
            {
                if (JsonSerializer.Deserialize<RunnerJson>(jsonText) is not { } data)
                {
                    throw new InvalidRLauncherConfigurationFileException()
                    {
                        FileType = ConfigurationFileTypes.Runner,
                        FileContent = jsonText
                    };
                }

                runner.LoadFrom(data);
            }
            catch (JsonException)
            {
                throw new InvalidRLauncherConfigurationFileException()
                {
                    FileType = ConfigurationFileTypes.Launcher,
                    FileContent = jsonText
                };
            }
        }
    }
}
