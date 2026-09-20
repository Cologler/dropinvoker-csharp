using System.Diagnostics;

using RLauncher.Abstractions;

namespace RLauncher.Internal;

class Runner(IRunnerData data) : BaseRunner
{
    private readonly RunnerDataSnapshot _data = new RunnerDataSnapshot(data);

    public override IReadOnlyList<string> GetCommand(ExecuteContext context)
    {
        ThrowIfNull(context);

        var executable = this._data?.Executable ?? throw new InvalidOperationException("the executable of runner is empty.");

        var runnerArguments = this._data?.Arguments?
            .Where(x => x is not null)
            .Cast<string>()
            .ToArray() ?? [];

        var arguments = this.ExpandArguments(context, runnerArguments, this.ExpandCommandArguments(context));
        return [executable, .. arguments];
    }

    public override Task<int> RunAsync(ExecuteContext context)
    {
        var command = this.GetCommand(context);
        var startInfo = new ProcessStartInfo
        {
            FileName = command[0]
        };
        foreach (var args in command.Skip(1))
        {
            startInfo.ArgumentList.Add(args);
        }

        var workingDirectory = context.Command.WorkingDirectory ?? this._data?.WorkingDirectory;
        if (workingDirectory is null)
        {
            startInfo.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(startInfo.FileName));
        }
        else
        {
            startInfo.WorkingDirectory = this.ExpandVariable(context, workingDirectory);
        }

        return this.RunAsync(startInfo);
    }
}
