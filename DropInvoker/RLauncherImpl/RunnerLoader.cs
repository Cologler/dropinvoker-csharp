using RLauncher;
using RLauncher.Exceptions;
using RLauncher.Json;
using RLauncher.Yaml;

using System;
using System.Collections.Generic;
using System.IO;

namespace DropInvoker.RLauncherImpl
{
    class RunnerLoader : IRunnerLoader
    {
        private readonly Dictionary<string, IRunner> _cache =
            new Dictionary<string, IRunner>(StringComparer.OrdinalIgnoreCase);

        public IRunner? GetRunner(string name)
        {
            if (!this._cache.TryGetValue(name, out var runner))
            {
                runner = this.LoadRunner(name);
                if (runner != null)
                {
                    this._cache.Add(name, runner);
                }
            }

            return runner;
        }

        private IRunner? LoadRunner(string name)
        {
            var runner = new Runner();
            var prefix = Path.Combine("runners", name);

            string? fileName = null;
            string? fileContent = null;

            try
            {
                fileName = prefix + ".yaml";
                if (File.Exists(fileName))
                {
                    fileContent = File.ReadAllText(fileName);
                    runner.LoadFromYaml(fileContent);
                    return runner;
                }

                fileName = prefix + ".json";
                if (File.Exists(fileName))
                {
                    fileContent = File.ReadAllText(fileName);
                    runner.LoadFromJson(fileContent);
                    return runner;
                }
            }
            catch (InvalidRLauncherConfigurationFileException exc)
            {
                exc.FileFullPath = Path.GetFullPath(fileName!);
                exc.FileContent = fileContent;
                throw;
            }

            return null;
        }
    }
}
