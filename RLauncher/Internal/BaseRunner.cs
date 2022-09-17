
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    abstract class BaseRunner : IRunner
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

            return this.ExpandArguments(context, context.Launcher.Arguments, context.Arguments);
        }

        protected IEnumerable<string> ExpandArguments(RunContext context, IEnumerable<string> rawArgs, IEnumerable<string> refArgs)
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
                        yield return this.DecodeArgument(context, a);
                    }
                }
                else
                {
                    yield return this.DecodeArgument(context, arg);
                }
            }
        }

        internal protected string DecodeArgument(RunContext context, in string argumentValue)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            if (argumentValue is null)
                throw new ArgumentNullException(nameof(argumentValue));

            var value = argumentValue;

            return Environment.ExpandEnvironmentVariables(value);
        }
    }
}
