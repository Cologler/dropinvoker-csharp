using DropInvoker.Models.Configurations;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DropInvoker.Models
{
    class RunnerLoader
    {
        private static readonly Dictionary<string, Runner> _cache =
            new Dictionary<string, Runner>(StringComparer.OrdinalIgnoreCase);

        public static Runner Load(string name)
        {
            if (!_cache.TryGetValue(name, out var launcher))
            {
                var path = Path.Combine("launchers", name + ".json");
                if (File.Exists(path))
                {
                    var text = File.ReadAllText(path);
                    var json = JsonSerializer.Deserialize<RunnerJson>(text);
                    launcher = new Runner(json);
                    _cache.Add(name, launcher);
                }
            }

            return launcher;
        }
    }
}
