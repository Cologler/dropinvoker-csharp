namespace RLauncher
{
    public interface ILauncherLoader
    {
        ILauncherData? TryGetTemplate(string name);
    }
}
