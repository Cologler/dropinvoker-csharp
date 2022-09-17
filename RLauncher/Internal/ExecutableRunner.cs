
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class ExecutableRunner : BaseRunner, IDefaultRunner
    {
        public override Task RunAsync(RunContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            
            var startInfo = new ProcessStartInfo();
            var arguments = this.ExpandArguments(context).ToArray();
            if (arguments.Length == 0)
            {
                throw new InvalidOperationException("arguments is empty.");
            }

            startInfo.FileName = arguments[0];
            foreach (var args in arguments[1..])
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
}
