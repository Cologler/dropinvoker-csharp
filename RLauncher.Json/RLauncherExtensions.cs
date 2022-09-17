using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;

using System;

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
    }
}
