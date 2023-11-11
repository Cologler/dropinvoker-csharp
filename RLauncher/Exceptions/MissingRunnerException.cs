namespace RLauncher.Exceptions;

public sealed class MissingRunnerException : Exception
{
    public MissingRunnerException(string runnerName)
    {
        this.RunnerName = runnerName;
    }

    public string RunnerName { get; }
}
