namespace RLauncher.Exceptions;

public sealed class MissingRunnerException(string runnerName) : Exception
{
    public string RunnerName { get; } = runnerName;
}
