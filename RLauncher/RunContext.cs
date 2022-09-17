
using System;
using System.Collections.Generic;
using System.Linq;

using RLauncher.Abstractions;

namespace RLauncher
{
    public class RunContext
    {
        public ICommand Command { get; }

        public IRunner Runner { get; }

        internal RunContext(ICommand launcher, IRunner runner, string[] arguments)
        {
            this.Command = launcher ?? throw new ArgumentNullException(nameof(launcher));
            this.Runner = runner;
            this.Arguments = arguments ?? Array.Empty<string>();
        }

        public IReadOnlyList<string> Arguments { get; }
    }
}
