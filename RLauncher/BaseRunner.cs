
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RLauncher
{
    public abstract class BaseRunner : IRunner
    {
        public abstract Task RunAsync(RunContext context);

        protected async Task RunAsync(ProcessStartInfo startInfo)
        {
            if (startInfo is null)
                throw new ArgumentNullException(nameof(startInfo));

            var tcs = new TaskCompletionSource<bool>();
            var proc = Process.Start(startInfo);
            proc.Exited += (_, __) => tcs.SetResult(true);
            await tcs.Task.ConfigureAwait(false); // ensure proc dispose after return;
        }

        protected IEnumerable<string> ExpandArguments(RunContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            return this.ExpandArguments(context.Launcher.Arguments, context.Arguments);
        }

        protected IEnumerable<string> ExpandArguments(IEnumerable<string?>? rawArgs, IEnumerable<string?>? refArgs)
        {
            rawArgs ??= Enumerable.Empty<string>();
            refArgs ??= Enumerable.Empty<string>();

            foreach (var arg in rawArgs.Where(z => z != null))
                if (arg == "$*")
                    foreach (var a in refArgs.Where(z => z != null))
                        yield return this.DecodeArgument(a!);
                else
                    yield return this.DecodeArgument(arg!);
        }

        internal protected string DecodeArgument(string value)
        {
            return Environment.ExpandEnvironmentVariables(value);
        }
    }
}
