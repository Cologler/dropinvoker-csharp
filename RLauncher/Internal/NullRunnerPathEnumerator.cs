using System.Collections.Generic;
using System.Linq;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class NullRunnerPathEnumerator : IRunnerPathEnumerator
    {
        public IAsyncEnumerable<string> EnumeratePathsAsync(string name) => AsyncEnumerable.Empty<string>();
    }
}
