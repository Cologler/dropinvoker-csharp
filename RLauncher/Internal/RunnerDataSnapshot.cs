using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class RunnerDataSnapshot(IRunnerData runnerData) : IRunnerData
    {
        public string? Executable { get; } = runnerData.Executable;

        public string?[]? Arguments { get; } = runnerData.Arguments;

        public string? WorkingDirectory { get; } = runnerData.WorkingDirectory;
    }
}
