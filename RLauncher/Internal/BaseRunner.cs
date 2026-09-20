using System.Diagnostics;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    abstract class BaseRunner : IRunner
    {
        public abstract IReadOnlyList<string> GetCommand(ExecuteContext context);

        public abstract Task RunAsync(ExecuteContext context);

        protected async Task RunAsync(ProcessStartInfo startInfo)
        {
            ThrowIfNull(startInfo);

            if (Process.Start(startInfo) is { } proc)
            {
                await proc.WaitForExitAsync().ConfigureAwait(false);
            }
        }

        protected IEnumerable<string> ExpandCommandArguments(ExecuteContext context)
        {
            ThrowIfNull(context);

            return this.ExpandArguments(context, context.Command.Arguments, context.Arguments);
        }

        protected IEnumerable<string> ExpandArguments(ExecuteContext context, IEnumerable<string> rawArgs, IEnumerable<string> refArgs)
        {
            ThrowIfNull(context);
            ThrowIfNull(rawArgs);
            ThrowIfNull(refArgs);

            foreach (var arg in rawArgs.Where(z => z != null))
            {
                if (arg == "$*")
                {
                    foreach (var a in refArgs)
                    {
                        yield return this.ExpandVariable(context, a);
                    }
                }
                else
                {
                    yield return this.ExpandVariable(context, arg);
                }
            }
        }

        protected internal string ExpandVariable(ExecuteContext context, in string argument)
        {
            ThrowIfNull(context);
            ThrowIfNull(argument);

            var value = argument;

            if (value == "~" || (value.Length >= 2 && value[..1] == "~" && "/\\".Contains(value[1])))
            {
                value = string.Concat("%USERPROFILE%", value.AsSpan(1));
            }

            return Environment.ExpandEnvironmentVariables(value);
        }
    }
}
