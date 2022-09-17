using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class CommandDataSnapshot : ICommandData
    {
        public CommandDataSnapshot(ICommandData launcherData)
        {
            this.Runner = launcherData.Runner;
            this.Arguments = launcherData.Arguments;
            this.WorkingDirectory = launcherData.WorkingDirectory;
            this.Name = launcherData.Name;
            this.Description = launcherData.Description;
            this.Accepts = launcherData.Accepts?.Clone() as string?[];
        }

        public string? Runner { get; }

        public string?[]? Arguments { get; }

        public string? WorkingDirectory { get; }

        public string? Name { get; }

        public string? Description { get; }

        public string?[]? Accepts { get; }
    }
}
