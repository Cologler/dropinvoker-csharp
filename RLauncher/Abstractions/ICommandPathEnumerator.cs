using System.Collections.Generic;

namespace RLauncher.Abstractions;

public interface ICommandPathEnumerator
{
    IAsyncEnumerable<string> EnumeratePathsAsync(string name);
}
