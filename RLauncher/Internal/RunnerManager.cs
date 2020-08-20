
using System;
using System.Collections.Generic;

namespace RLauncher.Internal
{
    internal class RunnerManager : IRunnerManager
    {
        private readonly IRunner _defaultRunner = new ExecutableRunner();
        private readonly Dictionary<string, IRunner> _runners = new Dictionary<string, IRunner>();

        public void UseRunner(string name, IRunner runner)
        {
            this._runners[name] = runner;
        }

        public IRunner GetDefaultRunner() => this._defaultRunner;

        public IRunner GetRunner(string name) => this._runners[name];
    }
}
