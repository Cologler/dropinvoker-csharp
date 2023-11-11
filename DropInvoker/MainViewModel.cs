using System.IO;

using DropInvoker.Models;

using PropertyChanged.SourceGenerator;

namespace DropInvoker;

partial class MainViewModel
{
    [Notify] Scene? _scene;

    public MainViewModel(AppDirectories directories)
    {
        try
        {
            this.SceneLoaders.AddRange(
                directories.GetScenesPath()
                .GetFiles()
                .Where(x => ".json".Equals(x.Extension, StringComparison.OrdinalIgnoreCase))
                .Select(z => new SceneLoader(z.FullName)));
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
        }
    }
}
