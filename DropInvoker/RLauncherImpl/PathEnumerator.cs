using System.IO;

using RLauncher.Abstractions;

namespace DropInvoker.RLauncherImpl
{
    class PathEnumerator(AppDirectories directories) : IRunnerPathEnumerator, ICommandPathEnumerator
    {
        IAsyncEnumerable<string> IRunnerPathEnumerator.EnumeratePathsAsync(string name)
        {
            var prefix = Path.Combine(directories.GetRunnersPath().FullName, name);

            return new[] { ".yaml", ".json", ".yml" }
                .Select(suffix => prefix + suffix)
                .Where(File.Exists)
                .ToAsyncEnumerable();
        }

        IAsyncEnumerable<string> ICommandPathEnumerator.EnumeratePathsAsync(string name)
        {
            var prefix = Path.Combine(directories.GetCommandsPath().FullName, name);

            return new[] { ".yaml", ".json", ".yml" }
                .Select(suffix => prefix + suffix)
                .Where(File.Exists)
                .ToAsyncEnumerable();
        }
    }
}
