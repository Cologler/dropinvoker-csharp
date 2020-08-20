
using RLauncher.Internal;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RLauncher
{
    public class Runner : BaseRunner
    {
        private IRunnerData? _data;

        public string? Executable => this._data?.Executable;

        public string?[]? Arguments => this._data?.Arguments;

        public string? WorkingDirectory => this._data?.WorkingDirectory;

        public override Task RunAsync(RunContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var startInfo = new ProcessStartInfo();

            var executable = this.Executable;
            if (executable is null)
            {
                throw new InvalidOperationException("the executable of runner is empty.");
            }

            startInfo.FileName = executable;
            var arguments = this.ExpandArguments(this.Arguments, this.ExpandArguments(context));
            foreach (var args in arguments)
                startInfo.ArgumentList.Add(args);

            var workingDirectory = this.WorkingDirectory ?? context.Launcher.WorkingDirectory;
            if (workingDirectory != null)
                startInfo.WorkingDirectory = this.DecodeArgument(workingDirectory);

            return this.RunAsync(startInfo);
        }

        public void LoadFrom(IRunnerData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            this._data = new RunnerDataSnapshot(data);
        }
    }
}
