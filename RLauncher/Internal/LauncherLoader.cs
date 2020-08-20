
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;

namespace RLauncher.Internal
{
    class LauncherLoader : ILauncherLoader
    {
        private readonly Dictionary<string, ILauncherData> _templates = new Dictionary<string, ILauncherData>();

        public LauncherLoader(IServiceProvider serviceProvider)
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

        public ILauncherData? TryGetTemplate(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            return this._templates.GetValueOrDefault(name);
        }

        public Launcher CreateLauncher() => this.ServiceProvider.GetRequiredService<Launcher>();
    }
}
