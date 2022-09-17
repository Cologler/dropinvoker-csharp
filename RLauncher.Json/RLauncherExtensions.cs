using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;
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
        public static IServiceCollection AddJsonModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton<IDocumentLoader<IRunnerData>, JsonDocumentLoader>();
            services.AddSingleton<IDocumentLoader<ILauncherData>, JsonDocumentLoader>();

            return services;
        }

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
    }
}
