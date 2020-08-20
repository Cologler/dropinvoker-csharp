
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RLauncher.Internal;

using System;

namespace RLauncher
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseRLauncher(this IServiceCollection services,
            ILauncherLoader launcherLoader)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (launcherLoader is null)
                throw new ArgumentNullException(nameof(launcherLoader));

            services.AddSingleton<IRunnerManager, RunnerManager>()
                .AddTransient<Launcher>()
                .AddSingleton(launcherLoader);

            return services;
        }
    }
}
