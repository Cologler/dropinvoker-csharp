using RLauncher.Abstractions;

namespace RLauncher;

class ExecuteContext
{
    public ICommand Command { get; }

    public IRunner Runner { get; }

    internal ExecuteContext(ICommand command, IRunner runner, string[] arguments)
    {
        ThrowIfNull(command);
        ThrowIfNull(runner);
        ThrowIfNull(arguments);

        this.Command = command;
        this.Runner = runner;
        this.Arguments = arguments;
    }

    public IReadOnlyList<string> Arguments { get; }
}
