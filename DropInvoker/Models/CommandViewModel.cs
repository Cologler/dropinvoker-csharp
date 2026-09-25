using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using PropertyChanged.SourceGenerator;

using RLauncher.Abstractions;
using RLauncher.Exceptions;

namespace DropInvoker.Models;

partial class CommandViewModel
{
    private const int StatusControlCExit = unchecked((int)0xC000013A); // -1073741510

    public static CommandViewModel Empty { get; } = new CommandViewModel(null);

    [Notify] string _description = string.Empty;
    [Notify] string _detailedDescription = string.Empty;

    public CommandViewModel(string? commandName)
    {
        this.CommandName = commandName;

        if (commandName is null)
        {
            this.IsEnabled = false;
        }
        else
        {
            this.Description = commandName;
            this.DetailedDescription = commandName;
            this.IsEnabled = true;

            _ = LoadCommandInfo();
        }

        async Task LoadCommandInfo()
        {
            if ((await this.LoadCommandAsync(commandName)) is { } command)
            {
                this.Description = command.Description;
                this.DetailedDescription = command.Description;
                try
                {
                    // Keep the input placeholder because dropped content is not known until execution.
                    var commandLine = await command.GetCommandAsync(["$*"]);
                    this.DetailedDescription = $"{command.Description}{Environment.NewLine}{string.Join(" ", commandLine.Select(QuoteArgument))}";
                }
                catch (Exception e)
                {
                    this.DetailedDescription = $"{command.Description}{Environment.NewLine}{e.Message}";
                }
            }
        }
    }

    public string? CommandName { get; }

    private static string QuoteArgument(string argument)
    {
        if (argument.Length > 0 && !argument.Any(c => char.IsWhiteSpace(c) || c == '"'))
            return argument;

        var result = new StringBuilder().Append('"');
        var backslashes = 0;
        foreach (var character in argument)
        {
            if (character == '\\')
            {
                backslashes++;
                continue;
            }

            // Windows parsing requires doubled backslashes before an escaped quote.
            result.Append('\\', character == '"' ? backslashes * 2 + 1 : backslashes);
            result.Append(character);
            backslashes = 0;
        }

        // Trailing backslashes must not escape the closing quote.
        return result.Append('\\', backslashes * 2).Append('"').ToString();
    }

    private ValueTask<ICommand?> LoadCommandAsync(string name)
    {
        var loader = ((App)Application.Current).ServiceProvider.GetRequiredService<ICommandLoader>();
        return loader.GetCommandAsync(name);
    }

    public bool IsEnabled { get; }

    private static void ShowErrorMessageBox(string message)
    {
        MessageBox.Show(Application.Current.MainWindow, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    async Task RunAsync(ICommand command, IEnumerable<string> args)
    {
        try
        {
            var exitCode = await command.RunAsync(args);
            if (exitCode != 0 && exitCode != StatusControlCExit)
            {
                ShowErrorMessageBox($"Command '{command.Name}' exited with code {exitCode}.");
            }
        }
        catch (MissingRunnerException mre)
        {
            ShowErrorMessageBox($"Runner not found: {mre.RunnerName}");
        }
        catch (Exception e)
        {
            ShowErrorMessageBox($"Catch exception when run the command:\n{e.Message}");
        }
    }

    public async Task RunAsync(IEnumerable<string> args)
    {
        var command = await this.LoadCommandAsync(this.CommandName!);

        if (command is null)
        {
            ShowErrorMessageBox($"Command not found: {this.CommandName}");
            return;
        }

        await this.RunAsync(command, args);
    }

    public async Task OnDropAsync(DragEventArgs eventArgs)
    {
        var command = await this.LoadCommandAsync(this.CommandName!);

        if (command is null)
        {
            ShowErrorMessageBox($"Command not found: {this.CommandName}");
            return;
        }

        var accepts = command.Accepts.ToHashSet(StringComparer.OrdinalIgnoreCase);

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
                await this.RunAsync(command, [data]);
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
