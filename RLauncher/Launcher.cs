
using Microsoft.Extensions.DependencyInjection;

using RLauncher.Internal;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RLauncher
{
    public class Launcher
    {
        private readonly IServiceProvider _serviceProvider;
        private ILauncherData? _launcherData;

        public Launcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public string?[]? Arguments => this._launcherData?.Arguments;

        public string? WorkingDirectory => this._launcherData?.WorkingDirectory;

        public string Name => this._launcherData?.Name ?? string.Empty;

        public string Description => this._launcherData?.Description ?? string.Empty;

        public string?[] Accepts => this._launcherData?.Accepts ?? Array.Empty<string>();

        public async Task RunAsync(IEnumerable<string?>? arguments = null)
        {
            var context = new RunContext(this, arguments?.ToArray());

            var runnerName = this._launcherData?.Runner;
            var runner = runnerName is null
                    ? this._serviceProvider.GetRequiredService<IDefaultRunner>()
                    : await this._serviceProvider.GetRequiredService<IRunnerLoader>().GetRunnerAsync(runnerName).ConfigureAwait(false)
                    ?? this._serviceProvider.GetRequiredService<IDefaultRunner>();

            await runner.RunAsync(context).ConfigureAwait(false);
        }

        public void LoadFrom(ILauncherData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var snapshot = new LauncherDataSnapshot(data);

            this._launcherData = snapshot;     
        }
    }
}
