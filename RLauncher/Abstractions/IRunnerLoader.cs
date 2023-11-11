namespace RLauncher.Abstractions;

interface IRunnerLoader
{
    public ValueTask<IRunner?> GetRunnerAsync(string name);
}
