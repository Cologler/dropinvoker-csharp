
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RLauncher.Abstractions;
using RLauncher.Internal;

using System;

namespace RLauncher
{
    public static class ServiceCollectionExtensions
    {
        internal const string DefaultRunnerKey = "default";

        public static IServiceCollection UseRLauncher(this IServiceCollection services)
        {
            ThrowIfNull(services);

            services
                .AddKeyedSingleton<IRunner, NullRunner>(DefaultRunnerKey)
                .AddSingleton<IRunnerLoader, RunnerLoader>()
                .AddSingleton<IRunnerPathEnumerator, NullPathEnumerator>()
                .AddSingleton<ICommandLoader, CommandLoader>()
                .AddSingleton<ICommandPathEnumerator, NullPathEnumerator>();

            return services;
        }
    }
}
