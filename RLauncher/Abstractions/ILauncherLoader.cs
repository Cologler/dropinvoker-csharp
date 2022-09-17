using System.Threading.Tasks;

namespace RLauncher.Abstractions;

public interface ILauncherLoader
{
    public ValueTask<Launcher?> GetLauncherAsync(string name);
}
