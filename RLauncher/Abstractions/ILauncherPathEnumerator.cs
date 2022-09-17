using System.Collections.Generic;

namespace RLauncher.Abstractions;

public interface ILauncherPathEnumerator
{
    IAsyncEnumerable<string> EnumeratePathsAsync(string name);
}
