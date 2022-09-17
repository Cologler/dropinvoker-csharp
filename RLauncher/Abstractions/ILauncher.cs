using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLauncher.Abstractions;

public interface ILauncher
{
    string Name { get; }

    string Description { get; }

    IReadOnlyList<string> Accepts { get; }

    string? WorkingDirectory { get; }

    IReadOnlyList<string> Arguments { get; }

    Task RunAsync(IEnumerable<string> arguments);
}
