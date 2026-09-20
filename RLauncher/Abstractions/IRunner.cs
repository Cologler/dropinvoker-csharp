namespace RLauncher.Abstractions;

interface IRunner
{
    IReadOnlyList<string> GetCommand(ExecuteContext context);

    Task<int> RunAsync(ExecuteContext context);
}
