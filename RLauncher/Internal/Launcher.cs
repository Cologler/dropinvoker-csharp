
using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLauncher.Internal
{
    class Launcher : ILauncher
    {
        private readonly IServiceProvider _serviceProvider;
        private ILauncherData? _launcherData;

        public Launcher(IServiceProvider serviceProvider, ILauncherData data)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _launcherData = new LauncherDataSnapshot(data);
        }

        public string Name => _launcherData?.Name ?? string.Empty;

        public string Description => _launcherData?.Description ?? string.Empty;

        public IReadOnlyList<string> Arguments => _launcherData?.Arguments?.Where(x => x is not null).Cast<string>().ToArray() ?? Array.Empty<string>();

        public string? WorkingDirectory => _launcherData?.WorkingDirectory;

        public IReadOnlyList<string> Accepts => _launcherData?.Accepts?.Where(x => x is not null).Cast<string>().ToArray() ?? Array.Empty<string>();

        public async Task RunAsync(IEnumerable<string> arguments)
        {
            ArgumentNullException.ThrowIfNull(arguments);

            var context = new RunContext(this, arguments.ToArray());

            var runnerName = _launcherData?.Runner;
            var runner = runnerName is null
                    ? _serviceProvider.GetRequiredService<IDefaultRunner>()
                    : await _serviceProvider.GetRequiredService<IRunnerLoader>().GetRunnerAsync(runnerName).ConfigureAwait(false)
                    ?? _serviceProvider.GetRequiredService<IDefaultRunner>();

            await runner.RunAsync(context).ConfigureAwait(false);
        }
    }
}
