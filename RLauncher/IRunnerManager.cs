using RLauncher.Internal;

namespace RLauncher
{
    public interface IRunnerManager
    {
        void UseRunner(string name, IRunner runner);

        IRunner GetRunner(string name);

        IRunner GetDefaultRunner();
    }
}
