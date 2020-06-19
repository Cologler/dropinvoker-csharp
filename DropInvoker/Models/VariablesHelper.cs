
using System;
using System.Collections.Generic;
using System.Linq;

namespace DropInvoker.Models
{
    static class VariablesHelper
    {
        public static IEnumerable<string> ExpandArguments(IEnumerable<string?> rawArgs, IEnumerable<string> toRefArgs)
        {
            if (rawArgs is null)
                throw new ArgumentNullException(nameof(rawArgs));
            if (toRefArgs is null)
                throw new ArgumentNullException(nameof(toRefArgs));

            foreach (var arg in rawArgs.Where(z => z != null))
            {
                if (arg == Constraints.ArgumentsReference)
                {
                    foreach (var a in toRefArgs)
                        yield return a;
                }
                else
                {
                    yield return Environment.ExpandEnvironmentVariables(arg!);
                }
            }
        }
    }
}
