
using System;
using System.Collections.Generic;
using System.Linq;

using RLauncher.Abstractions;

namespace RLauncher
{
    public class RunContext
    {
        public ILauncher Launcher { get; }

        public IRunner Runner { get; }

        internal RunContext(ILauncher launcher, IRunner runner, string[] arguments)
        {
            this.Launcher = launcher ?? throw new ArgumentNullException(nameof(launcher));
            this.Runner = runner;
            this.Arguments = arguments ?? Array.Empty<string>();
        }

        public IReadOnlyList<string> Arguments { get; }
    }
}
