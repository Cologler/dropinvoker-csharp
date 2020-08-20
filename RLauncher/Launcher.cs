﻿
using Microsoft.Extensions.DependencyInjection;

using RLauncher.Internal;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLauncher
{
    public class Launcher
    {
        private readonly IServiceProvider _serviceProvider;
        private ILauncherData? _launcherData;

        public Launcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IRunner Runner
        {
            get
            {
                var name = this._launcherData?.Runner;
                var runnerManager = this._serviceProvider.GetRequiredService<IRunnerManager>();
                return name is null ? runnerManager.GetDefaultRunner() : runnerManager.GetRunner(name);
            }
        }

        public ILauncherData? Template { get; private set; }

        public string?[]? Arguments => this._launcherData?.Arguments ?? this.Template?.Arguments;

        public string? WorkingDirectory => this._launcherData?.WorkingDirectory ?? this.Template?.WorkingDirectory;

        public string Name => this._launcherData?.Name ?? string.Empty;

        public string Description => this._launcherData?.Description ?? this.Template?.Description ?? string.Empty;

        public string?[] Accepts => this._launcherData?.Accepts ?? this.Template?.Accepts ?? Array.Empty<string>();

        public Task RunAsync(IEnumerable<string?>? arguments = null)
        {
            var context = new RunContext(this, arguments?.ToArray());
            return this.Runner.RunAsync(context);
        }

        public void LoadFrom(ILauncherData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var snapshot = new LauncherDataSnapshot(data);

            if (snapshot.Runner != null)
            {
                var runnerManager = this._serviceProvider.GetRequiredService<IRunnerManager>();
                runnerManager.GetRunner(snapshot.Runner); // ensure we can get runner without exception.
            }

            this._launcherData = snapshot;

            var template = snapshot.Template;
            if (template is null)
            {
                this.Template = null;
            }
            else
            {
                this.Template = this._serviceProvider.GetRequiredService<ILauncherLoader>()
                    .TryGetTemplate(template);
            }             
        }
    }
}
