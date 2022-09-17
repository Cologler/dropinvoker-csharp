
using System;
using System.Collections.Generic;

using RLauncher.Abstractions;

namespace RLauncher
{
    public class RunContext
    {
        public ILauncher Launcher { get; }

        internal RunContext(ILauncher launcher, string[] arguments)
        {
            this.Launcher = launcher ?? throw new ArgumentNullException(nameof(launcher));
            this.Arguments = arguments ?? Array.Empty<string>();
        }

        public IReadOnlyList<string> Arguments { get; }
    }
}
