using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;

using System;

namespace RLauncher.Yaml
{
    public static class RLauncherExtensions
    {
        public static IServiceCollection AddYamlModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton<IDocumentLoader<IRunnerData>, YamlDocumentLoader>();
            services.AddSingleton<IDocumentLoader<ICommandData>, YamlDocumentLoader>();

            return services;
        }
    }
}
