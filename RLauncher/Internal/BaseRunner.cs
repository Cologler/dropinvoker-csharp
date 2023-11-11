using System.Diagnostics;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    abstract class BaseRunner : IRunner
    {
        public abstract Task RunAsync(ExecuteContext context);

        protected async Task RunAsync(ProcessStartInfo startInfo)
        {
            if (startInfo is null)
                throw new ArgumentNullException(nameof(startInfo));

            if (Process.Start(startInfo) is { } proc)
            {
                await proc.WaitForExitAsync().ConfigureAwait(false);
            }
        }

        protected IEnumerable<string> ExpandCommandArguments(ExecuteContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return this.ExpandArguments(context, context.Command.Arguments, context.Arguments);
        }

        protected IEnumerable<string> ExpandArguments(ExecuteContext context, IEnumerable<string> rawArgs, IEnumerable<string> refArgs)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (rawArgs is null)
                throw new ArgumentNullException(nameof(rawArgs));
            if (refArgs is null)
                throw new ArgumentNullException(nameof(refArgs));

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
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (argument is null)
                throw new ArgumentNullException(nameof(argument));

            var value = argument;

            if (value == "~" || (value.Length >= 2 && value[..1] == "~" && "/\\".Contains(value[1])))
            {
                value = string.Concat("%USERPROFILE%", value.AsSpan(1));
            }

            return Environment.ExpandEnvironmentVariables(value);
        }
    }
}
