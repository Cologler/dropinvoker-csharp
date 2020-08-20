using DropInvoker.Models.Configurations;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DropInvoker.Models
{
    class RunnerLoader
    {
        private static readonly Dictionary<string, Launcher> _cache =
            new Dictionary<string, Launcher>(StringComparer.OrdinalIgnoreCase);

        public static Launcher Load(string name)
        {
            if (!_cache.TryGetValue(name, out var launcher))
            {
                var path = Path.Combine("launchers", name + ".json");
                if (File.Exists(path))
                {
                    var text = File.ReadAllText(path);
                    var json = JsonSerializer.Deserialize<LauncherJson>(text);
                    launcher = new Launcher(json);
                    _cache.Add(name, launcher);
                }
            }

            return launcher;
        }
    }
}
