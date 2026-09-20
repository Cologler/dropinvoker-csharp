namespace RLauncher.Abstractions;

interface IRunner
{
    IReadOnlyList<string> GetCommand(ExecuteContext context);

    Task RunAsync(ExecuteContext context);
}
