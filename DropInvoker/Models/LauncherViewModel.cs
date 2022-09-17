using DropInvoker.Models.Configurations;

using Microsoft.Extensions.DependencyInjection;

using RLauncher;
using RLauncher.Json;
using RLauncher.Yaml;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DropInvoker.Models
{
    class LauncherViewModel
    {
        private readonly HashSet<string> _accepts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public LauncherViewModel(string? name)
        {
            if (name is null)
            {
                this.Description = string.Empty;
                this.IsEnabled = false;
            }
            else
            {
                this.Description = name;
                this.IsEnabled = true;

                this.Launcher = LoadLauncher(name);
                if (this.Launcher != null)
                {
                    this.Description = this.Launcher.Description;
                    foreach (var accept in this.Launcher.Accepts)
                    {
                        if (accept != null)
                            this._accepts.Add(accept);
                    }
                }
            }
        }

        private static Launcher? LoadLauncher(string name)
        {
            var launcher = ((App)Application.Current).ServiceProvider.GetRequiredService<Launcher>();

            var prefix = Path.Combine("launchers", name);

            var yamlPath = prefix + ".yaml";
            if (File.Exists(yamlPath))
            {
                launcher.LoadFromYaml(File.ReadAllText(yamlPath));
                return launcher;
            }

            var jsonPath = prefix + ".json";
            if (File.Exists(jsonPath))
            {
                launcher.LoadFromJson(File.ReadAllText(jsonPath));
                return launcher;
            }

            return null;
        }

        public Launcher? Launcher { get; }

        public string Description { get; }

        public bool IsEnabled { get; }

        public static LauncherViewModel Empty { get; } = new LauncherViewModel(null);

        private void ShowMessageBox(string message)
        {
            MessageBox.Show(Application.Current.MainWindow, message);
        }

        public Task RunAsync(IEnumerable<string> args)
        {
            if (this.Launcher is null)
            {
                this.ShowMessageBox($"unable load config file: {this.Description}");
                return Task.CompletedTask;
            }

            try
            {
                return this.Launcher.RunAsync(args);
            }
            catch (Exception e)
            {
                this.ShowMessageBox($"catch exception when run the launcher: \n{e.Message}.");
            }

            return Task.CompletedTask;
        }

        public Task OnDropAsync(DragEventArgs eventArgs)
        {
            if (this.Launcher is null)
            {
                this.ShowMessageBox($"unable load config file: {this.Description}");
                return Task.CompletedTask;
            }

            if (eventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dataStringArray = (string[])eventArgs.Data.GetData(DataFormats.FileDrop);
                Debug.Assert(dataStringArray.Length > 0);

                if (TestFileDrop(this._accepts, dataStringArray))
                {
                    return this.Launcher.RunAsync(dataStringArray);
                }
            }

            if (eventArgs.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                if (this._accepts.Contains(Accepts.Text))
                {
                    var data = (string)eventArgs.Data.GetData(DataFormats.UnicodeText);
                    return this.Launcher.RunAsync(new string[] { data });
                }
            }

            return Task.CompletedTask;

            static bool TestFileDrop(IReadOnlySet<string> accepts, string[] paths)
            {
                Debug.Assert(paths.Length > 0);

                var files = new List<string>(paths.Length);
                var dirs = new List<string>(paths.Length);
                var others = new List<string>();

                foreach (var item in paths)
                {
                    if (File.Exists(item))
                    {
                        files.Add(item);
                    } 
                    else if (Directory.Exists(item))
                    {
                        dirs.Add(item);
                    }
                    else
                    {
                        others.Add(item);
                    } 
                }

                if (others.Count > 0)
                    return false;

                if (files.Count == 0)
                {
                    Debug.Assert(dirs.Count == paths.Length);

                    if (accepts.Contains(Accepts.Dirs))
                        return true;

                    if (accepts.Contains(Accepts.Dir) && dirs.Count == 1)
                        return true;
                }

                if (dirs.Count == 0)
                {
                    Debug.Assert(files.Count == paths.Length);

                    if (accepts.Contains(Accepts.Files))
                        return true;

                    if (accepts.Contains(Accepts.File) && files.Count == 1)
                        return true;
                }

                return false;
            }
        }
    }
}
