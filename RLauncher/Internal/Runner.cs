﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class Runner : BaseRunner
    {
        private readonly IRunnerData _data;

        public Runner(IRunnerData data) => this._data = new RunnerDataSnapshot(data);

        public override Task RunAsync(RunContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var startInfo = new ProcessStartInfo();

            startInfo.FileName = this._data?.Executable ?? throw new InvalidOperationException("the executable of runner is empty.");

            var runnerArguments = this._data?.Arguments?
                .Where(x => x is not null)
                .Cast<string>()
                .ToArray() ?? Array.Empty<string>();

            var arguments = this.ExpandArguments(context, runnerArguments, this.ExpandArguments(context));
            foreach (var args in arguments)
            {
                startInfo.ArgumentList.Add(args);
            }

            var workingDirectory = context.Launcher.WorkingDirectory ?? this._data?.WorkingDirectory;
            if (workingDirectory is null)
            {
                startInfo.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(startInfo.FileName));
            }
            else
            {
                startInfo.WorkingDirectory = this.DecodeArgument(context, workingDirectory);
            }

            return this.RunAsync(startInfo);
        }
    }
}
