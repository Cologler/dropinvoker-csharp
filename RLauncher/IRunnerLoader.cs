using System.Threading.Tasks;

namespace RLauncher
{
    public interface IRunnerLoader
    {
        public ValueTask<IRunner?> GetRunnerAsync(string name);
    }
}
