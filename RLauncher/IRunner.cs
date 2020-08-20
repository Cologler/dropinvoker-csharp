using System.Threading.Tasks;

namespace RLauncher
{
    public interface IRunner
    {
        Task RunAsync(RunContext context);
    }
}
