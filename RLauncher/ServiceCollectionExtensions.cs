
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RLauncher.Abstractions;
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
                .AddSingleton<IDefaultRunner, NullRunner>()
                .AddSingleton<IRunnerLoader, RunnerLoader>()
                .AddSingleton<IRunnerPathEnumerator, NullPathEnumerator>()
                .AddSingleton<ICommandLoader, CommandLoader>()
                .AddSingleton<ICommandPathEnumerator, NullPathEnumerator>();

            return services;
        }
    }
}
