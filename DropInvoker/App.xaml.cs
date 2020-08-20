using DropInvoker.RLauncherImpl;

using Microsoft.Extensions.DependencyInjection;

using RLauncher;

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
                .AddSingleton<ITemplateLoader, TemplateLoader>()
                .AddSingleton<IRunnerLoader, RunnerLoader>()
                .BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}
