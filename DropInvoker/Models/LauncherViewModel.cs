using DropInvoker.Models.Configurations;

using Microsoft.Extensions.DependencyInjection;

using PropertyChanged.SourceGenerator;

using RLauncher;
using RLauncher.Abstractions;
using RLauncher.Exceptions;
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

using YamlDotNet.Core.Tokens;

namespace DropInvoker.Models
{
    partial class CommandViewModel
    {
        public static CommandViewModel Empty { get; } = new CommandViewModel(null);

        [Notify] string _description = string.Empty;

        public CommandViewModel(string? launcherName)
        {
            this.CommandName = launcherName;

            if (launcherName is null)
            {
                this.IsEnabled = false;
            }
            else
            {
                this.Description = launcherName;
                this.IsEnabled = true;

                _ = LoadCommandInfo();
            }

            async Task LoadCommandInfo()
            {
                if ((await LoadCommandAsync(launcherName!)) is { } launcher)
                {
                    this.Description = launcher.Description;
                }
            }
        }

        public string? CommandName { get; }

        private ValueTask<ICommand?> LoadCommandAsync(string name)
        {
            var loader = ((App)Application.Current).ServiceProvider.GetRequiredService<ICommandLoader>();
            return loader.GetCommandAsync(name);
        }

        public bool IsEnabled { get; }

        private void ShowMessageBox(string message)
        {
            MessageBox.Show(Application.Current.MainWindow, message);
        }

        Task RunAsync(ICommand command, IEnumerable<string> args)
        {
            try
            {
                return command.RunAsync(args);
            }
            catch (Exception e)
            {
                this.ShowMessageBox($"Catch exception when run the command: \n{e.Message}.");
            }

            return Task.CompletedTask;
        }

        public async Task RunAsync(IEnumerable<string> args)
        {
            var command = await LoadCommandAsync(this.CommandName!);

            if (command is null)
            {
                this.ShowMessageBox($"Unable load command with name: {this.CommandName}");
                return;
            }

            await this.RunAsync(command, args);
        }

        public async Task OnDropAsync(DragEventArgs eventArgs)
        {
            var command = await LoadCommandAsync(this.CommandName!);

            if (command is null)
            {
                this.ShowMessageBox($"Unable load command with name: {this.CommandName}");
                return;
            }

            var accepts = command.Accepts?.Where(x => x is { }).Cast<string>().ToHashSet() ?? new HashSet<string>();

            if (eventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dataStringArray = (string[])eventArgs.Data.GetData(DataFormats.FileDrop);
                Debug.Assert(dataStringArray.Length > 0);

                if (TestFileDrop(accepts, dataStringArray))
                {
                    await this.RunAsync(command, dataStringArray);
                    return;
                }
            }

            if (eventArgs.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                if (accepts.Contains(Accepts.Text))
                {
                    var data = (string)eventArgs.Data.GetData(DataFormats.UnicodeText);
                    await this.RunAsync(command, new string[] { data });
                    return;
                }
            }

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
