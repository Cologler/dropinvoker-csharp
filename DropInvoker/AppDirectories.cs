using System.IO;

namespace DropInvoker;

internal class AppDirectories
{
    public DirectoryInfo AppDataDirectory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).CreateSubdirectory("DropInvoker");

    public DirectoryInfo GetScenesPath() => AppDataDirectory.CreateSubdirectory("scenes");

    public DirectoryInfo GetRunnersPath() =>  AppDataDirectory.CreateSubdirectory("runners");

    public DirectoryInfo GetCommandsPath() => AppDataDirectory.CreateSubdirectory("commands");
}
