
using System;

namespace RLauncher
{
    public class RunContext
    {
        public Launcher Launcher { get; }

        public RunContext(Launcher launcher, string?[]? arguments = null)
        {
            this.Launcher = launcher ?? throw new ArgumentNullException(nameof(launcher));
            this.Arguments = arguments ?? Array.Empty<string>();
        }

        public string?[] Arguments { get; }
    }
}
