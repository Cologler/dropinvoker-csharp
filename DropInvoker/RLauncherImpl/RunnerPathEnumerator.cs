using RLauncher.Abstractions;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DropInvoker.RLauncherImpl
{
    class RunnerPathEnumerator : IRunnerPathEnumerator
    {
        private readonly AppDirectories _directories;

        public RunnerPathEnumerator(AppDirectories directories)
        {
            this._directories = directories;
        }

        public IAsyncEnumerable<string> EnumeratePathsAsync(string name)
        {
            var prefix = Path.Combine(this._directories.GetRunnersPath(), name);

            return new[] { ".yaml", ".json", ".yml" }
                .Select(suffix => prefix + suffix)
                .Where(File.Exists)
                .ToAsyncEnumerable();
        }
    }
}
