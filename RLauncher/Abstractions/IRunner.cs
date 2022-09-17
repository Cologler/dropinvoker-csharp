using System.Threading.Tasks;

namespace RLauncher.Abstractions
{
    public interface IRunner
    {
        Task RunAsync(RunContext context);
    }
}
