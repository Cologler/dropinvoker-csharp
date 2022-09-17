using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;
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
        public static IServiceCollection AddYamlModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton<IDocumentLoader<IRunnerData>, YamlDocumentLoader>();
            services.AddSingleton<IDocumentLoader<ILauncherData>, YamlDocumentLoader>();

            return services;
        }

        public static void LoadFromYaml(this Launcher launcher, string yamlText)
        {
            if (launcher is null)
                throw new ArgumentNullException(nameof(launcher));

            launcher.LoadFrom(YamlUtils.ToLauncherData(yamlText));
        }
    }
}
