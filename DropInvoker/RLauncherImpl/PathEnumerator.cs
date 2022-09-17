﻿using RLauncher.Abstractions;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DropInvoker.RLauncherImpl
{
    class PathEnumerator : IRunnerPathEnumerator, ILauncherPathEnumerator
    {
        private readonly AppDirectories _directories;

        public PathEnumerator(AppDirectories directories)
        {
            this._directories = directories;
        }

        IAsyncEnumerable<string> IRunnerPathEnumerator.EnumeratePathsAsync(string name)
        {
            var prefix = Path.Combine(this._directories.GetRunnersPath(), name);

            return new[] { ".yaml", ".json", ".yml" }
                .Select(suffix => prefix + suffix)
                .Where(File.Exists)
                .ToAsyncEnumerable();
        }

        IAsyncEnumerable<string> ILauncherPathEnumerator.EnumeratePathsAsync(string name)
        {
            var prefix = Path.Combine(this._directories.GetLaunchersPath(), name);

            return new[] { ".yaml", ".json", ".yml" }
                .Select(suffix => prefix + suffix)
                .Where(File.Exists)
                .ToAsyncEnumerable();
        }
    }
}