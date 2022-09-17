using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class RunnerDataSnapshot : IRunnerData
    {
        public RunnerDataSnapshot(IRunnerData runnerData)
        {
            this.Executable = runnerData.Executable;
            this.Arguments = runnerData.Arguments;
            this.WorkingDirectory = runnerData.WorkingDirectory;
        }

        public string? Executable { get; }

        public string?[]? Arguments { get; }

        public string? WorkingDirectory { get; }
    }
}
