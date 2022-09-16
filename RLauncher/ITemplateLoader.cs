namespace RLauncher
{
    public interface ITemplateLoader
    {
        ILauncherData? GetTemplate(string name);
    }
}
