namespace RLauncher.Abstractions;

interface IRunner
{
    Task RunAsync(ExecuteContext context);
}
