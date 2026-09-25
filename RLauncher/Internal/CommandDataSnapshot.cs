using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class CommandDataSnapshot(ICommandData launcherData) : ICommandData
    {
        public string? Runner { get; } = launcherData.Runner;

        public string?[]? Arguments { get; } = launcherData.Arguments;

        public string? WorkingDirectory { get; } = launcherData.WorkingDirectory;

        public string? Name { get; } = launcherData.Name;

        public string? Description { get; } = launcherData.Description;

        public string?[]? Accepts { get; } = launcherData.Accepts?.Clone() as string?[];

        public bool IgnoreExitCode { get; } = launcherData.IgnoreExitCode;
    }
}
