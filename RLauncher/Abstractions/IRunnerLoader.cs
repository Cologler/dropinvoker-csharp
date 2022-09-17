using System.Threading.Tasks;

namespace RLauncher.Abstractions
{
    public interface IRunnerLoader
    {
        public ValueTask<IRunner?> GetRunnerAsync(string name);
    }
}
