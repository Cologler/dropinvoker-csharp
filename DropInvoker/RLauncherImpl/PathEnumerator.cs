using System.IO;

using RLauncher.Abstractions;

namespace DropInvoker.RLauncherImpl
{
    class PathEnumerator : IRunnerPathEnumerator, ICommandPathEnumerator
    {
        private readonly AppDirectories _directories;

        public PathEnumerator(AppDirectories directories)
        {
            this._directories = directories;
        }

        IAsyncEnumerable<string> IRunnerPathEnumerator.EnumeratePathsAsync(string name)
        {
            var prefix = Path.Combine(this._directories.GetRunnersPath().FullName, name);

            return new[] { ".yaml", ".json", ".yml" }
                .Select(suffix => prefix + suffix)
                .Where(File.Exists)
                .ToAsyncEnumerable();
        }

        IAsyncEnumerable<string> ICommandPathEnumerator.EnumeratePathsAsync(string name)
        {
            var prefix = Path.Combine(this._directories.GetCommandsPath().FullName, name);

            return new[] { ".yaml", ".json", ".yml" }
                .Select(suffix => prefix + suffix)
                .Where(File.Exists)
                .ToAsyncEnumerable();
        }
    }
}
