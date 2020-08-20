using RLauncher;
using RLauncher.Json;
using RLauncher.Yaml;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DropInvoker.RLauncherImpl
{
    class TemplateLoader : ITemplateLoader
    {
        private readonly Dictionary<string, ILauncherData> _templates = new Dictionary<string, ILauncherData>();

        public TemplateLoader(IServiceProvider serviceProvider)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public void AddTemplate(string name, ILauncherData launcherData)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (launcherData is null)
                throw new ArgumentNullException(nameof(launcherData));
            this._templates[name] = launcherData;
        }

        public ILauncherData? GetTemplate(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (!this._templates.TryGetValue(name, out var template))
            {
                template = this.LoadTemplate(name);
                if (template != null)
                {
                    this._templates.Add(name, template);
                }
            }

            return template;
        }

        private ILauncherData? LoadTemplate(string name)
        {
            var prefix = Path.Combine("templates", name);

            var yamlPath = prefix + ".yaml";
            if (File.Exists(yamlPath))
            {
                return YamlUtils.ToLauncherData(File.ReadAllText(yamlPath));
            }

            var jsonPath = prefix + ".json";
            if (File.Exists(jsonPath))
            {
                return JsonUtils.ToLauncherData(File.ReadAllText(jsonPath));
            }

            return null;
        }
    }
}
