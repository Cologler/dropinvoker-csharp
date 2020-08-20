using DropInvoker.Models.Configurations;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace DropInvoker.Models
{
    class Invoker
    {
        private readonly InvokerJson? _json;
        private readonly HashSet<string> _accepts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Launcher _launcher;

        public Invoker(string? name)
        {
            if (name is null)
            {
                this.Description = string.Empty;
            }
            else
            {
                this.Description = name;

                var path = Path.Combine("invokers", name + ".json");
                if (File.Exists(path))
                {
                    var text = File.ReadAllText(path);
                    var json = JsonSerializer.Deserialize<InvokerJson>(text);
                    this._json = json;

                    this.Description = json.Description ?? this.Description;
                    if (json.Accept != null)
                    {
                        foreach (var a in json.Accept)
                        {
                            if (a != null)
                                this._accepts.Add(a);
                        }
                    }

                    if (json.Launcher != null)
                    {
                        this._launcher = LauncherLoader.Load(json.Launcher);
                    }
                }
            }
        }

        public string Description { get; }

        public static Invoker Empty { get; } = new Invoker(null);

        private void ShowMessageBox(string message)
        {
            MessageBox.Show(Application.Current.MainWindow, message);
        }

        public void OnDrop(DragEventArgs eventArgs)
        {
            if (this._json is null)
            {
                ShowMessageBox($"unable load config file: {this.Description}");
                return;
            }

            void Invoke(IEnumerable<string> args)
            {
                var json = this._json!;
                var s = new ProcessStartInfo();
                s.FileName = json.Application ?? this._launcher.Application;

                var invokerArgs = VariablesHelper.ExpandArguments(json.Args ?? Array.Empty<string>(), args);
                foreach (var arg in this._launcher.ExpandArguments(invokerArgs))
                {
                    s.ArgumentList.Add(arg);
                }

                var workDir = json.WorkDir ?? this._launcher.WorkDir;
                if (workDir != null)
                {
                    s.WorkingDirectory = Environment.ExpandEnvironmentVariables(workDir);
                }

                try
                {
                    using var _ = Process.Start(s);
                }
                catch (Exception e)
                {
                    var msg = new StringBuilder()
                        .AppendLine("run app:")
                        .AppendLine()
                        .AppendLine("    " + s.FileName);
                    foreach (var a in s.ArgumentList)
                    {
                        msg.AppendLine("    - " + a);
                    }
                    msg.AppendLine()
                        .AppendLine("raise error:")
                        .AppendLine()
                        .AppendLine(e.Message);

                    ShowMessageBox(msg.ToString());
                }
            }

            if (eventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dataStringArray = (string[])eventArgs.Data.GetData(DataFormats.FileDrop);
                Debug.Assert(dataStringArray.Length > 0);

                var pathInfos = dataStringArray.Select(z =>
                {
                    var t = 0;
                    if (File.Exists(z))
                        t = 1;
                    if (Directory.Exists(z))
                        t = 2;
                    return (Type: t, Path: z);
                }).ToArray();

                if (pathInfos.Length == 1)
                {
                    if (pathInfos[0].Type == 1)
                    {
                        if (this._accepts.Contains(Accepts.File) || this._accepts.Contains(Accepts.Files))
                        {
                            Invoke(dataStringArray);
                        }
                    }
                    else if (pathInfos[0].Type == 2)
                    {
                        if (this._accepts.Contains(Accepts.Dir) || this._accepts.Contains(Accepts.Dirs))
                        {
                            Invoke(dataStringArray);
                        }
                    }
                }
                else
                {
                    var types = pathInfos.Select(z => z.Type).Distinct().ToArray();
                    if (types.Length == 1)
                    {
                        if (types[0] == 1 && this._accepts.Contains(Accepts.Files))
                        {
                            Invoke(dataStringArray);
                        }
                        else if (pathInfos[0].Type == 2 && this._accepts.Contains(Accepts.Dirs))
                        {
                            Invoke(dataStringArray);
                        }
                    }
                    else if (types.Length == 2 && !types.Contains(0))
                    {
                        if (this._accepts.Contains(Accepts.Files) && this._accepts.Contains(Accepts.Dirs))
                        {
                            Invoke(dataStringArray);
                        }
                    }
                }
            }

            if (eventArgs.Data.GetDataPresent(DataFormats.UnicodeText))
            {
                if (this._accepts.Contains(Accepts.Text))
                {
                    var data = (string)eventArgs.Data.GetData(DataFormats.UnicodeText);
                    Invoke(new string[] { data });
                }
            }
        }
    }
}
