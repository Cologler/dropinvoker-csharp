using DropInvoker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace DropInvoker
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            try
            {
                this.SceneLoaders.AddRange(
                    Directory.GetFiles("scenes").Select(z => new SceneLoader(z))
                );
            }
            catch (DirectoryNotFoundException)
            {
                // pass
            }
        }

        public List<SceneLoader> SceneLoaders { get; } = new List<SceneLoader>();

        public SceneLoader SelectedSceneLoader
        {
            set
            {
                if (value is null)
                    return;

                this.Scene = value.Load();
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Scene)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Scene? Scene { get; private set; }
    }
}
