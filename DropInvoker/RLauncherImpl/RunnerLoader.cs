using RLauncher;
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

            var yamlPath = prefix + ".yaml";
            if (File.Exists(yamlPath))
            {
                runner.LoadFromYaml(File.ReadAllText(yamlPath));
                return runner;
            }

            var jsonPath = prefix + ".json";
            if (File.Exists(jsonPath))
            {
                runner.LoadFromJson(File.ReadAllText(jsonPath));
                return runner;
            }

            return null;
        }
    }
}
