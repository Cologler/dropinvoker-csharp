using System.Threading.Tasks;

namespace RLauncher.Abstractions;

public interface ICommandLoader
{
    public ValueTask<ICommand?> GetCommandAsync(string name);
}
