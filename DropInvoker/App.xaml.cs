using DropInvoker.RLauncherImpl;

using Microsoft.Extensions.DependencyInjection;

using RLauncher;
using RLauncher.Abstractions;
using RLauncher.Json;
using RLauncher.Yaml;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DropInvoker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; } = default!;

        protected override void OnStartup(StartupEventArgs e)
        {
            this.ServiceProvider = new ServiceCollection()
                .UseRLauncher()
                .AddJsonModule()
                .AddYamlModule()
                .AddSingleton<ITemplateLoader, TemplateLoader>()
                .AddSingleton<IRunnerPathEnumerator, PathEnumerator>()
                .AddSingleton<ILauncherPathEnumerator, PathEnumerator>()
                .AddSingleton<MainViewModel>()
                .AddSingleton<AppDirectories>()
                .BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}
