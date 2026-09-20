using System.Diagnostics;

using RLauncher.Abstractions;

namespace RLauncher.Internal;

class NullRunner : BaseRunner
{
    public override IReadOnlyList<string> GetCommand(ExecuteContext context)
    {
        ThrowIfNull(context);

        var arguments = this.ExpandCommandArguments(context).ToArray();
        if (arguments.Length == 0)
        {
            throw new InvalidOperationException("arguments is empty.");
        }

        return arguments;
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

        var workingDirectory = context.Command.WorkingDirectory;
        if (workingDirectory is null)
        {
            startInfo.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(startInfo.FileName));
        }
        else
        {
            startInfo.WorkingDirectory = this.ExpandVariable(context, workingDirectory);
        }

        return base.RunAsync(startInfo);
    }
}
