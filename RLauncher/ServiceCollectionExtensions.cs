
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RLauncher.Internal;

using System;

namespace RLauncher
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseRLauncher(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            services
                .AddSingleton<IDefaultRunner, ExecutableRunner>()
                .AddTransient<Launcher>()
                .AddSingleton<IRunnerLoader, NullRunnerLoader>();

            return services;
        }
    }
}
