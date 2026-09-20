using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLauncher.Abstractions;

public interface ICommand
{
    string Name { get; }

    string Description { get; }

    IReadOnlyList<string> Accepts { get; }

    string? WorkingDirectory { get; }

    IReadOnlyList<string> Arguments { get; }

    /// <summary>Gets the executable and expanded arguments without running the command.</summary>
    /// <param name="arguments">The input arguments to substitute for <c>$*</c>.</param>
    /// <returns>The executable followed by its arguments.</returns>
    ValueTask<IReadOnlyList<string>> GetCommandAsync(IEnumerable<string> arguments);

    Task RunAsync(IEnumerable<string> arguments);
}
