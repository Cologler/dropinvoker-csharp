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

    /// <summary>Gets whether a nonzero process exit code should be ignored by the caller.</summary>
    bool IgnoreExitCode { get; }

    /// <summary>Gets the executable and expanded arguments without running the command.</summary>
    /// <param name="arguments">The input arguments to substitute for <c>$*</c>.</param>
    /// <returns>The executable followed by its arguments.</returns>
    ValueTask<IReadOnlyList<string>> GetCommandAsync(IEnumerable<string> arguments);

    /// <summary>Runs the command and waits for the process to exit.</summary>
    /// <param name="arguments">The input arguments to substitute for <c>$*</c>.</param>
    /// <returns>The process exit code.</returns>
    Task<int> RunAsync(IEnumerable<string> arguments);
}
