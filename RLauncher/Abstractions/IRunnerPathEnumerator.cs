using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLauncher.Abstractions;

public interface IRunnerPathEnumerator
{
    IAsyncEnumerable<string> EnumeratePathsAsync(string name);
}
