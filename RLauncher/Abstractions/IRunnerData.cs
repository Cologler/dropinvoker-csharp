namespace RLauncher.Abstractions
{
    public interface IRunnerData
    {
        public string? Executable { get; }

        public string?[]? Arguments { get; }

        public string? WorkingDirectory { get; }
    }
}
