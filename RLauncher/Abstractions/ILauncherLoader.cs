using System.Threading.Tasks;

namespace RLauncher.Abstractions;

public interface ILauncherLoader
{
    public ValueTask<ILauncher?> GetLauncherAsync(string name);
}
