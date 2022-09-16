namespace RLauncher
{
    public interface IRunnerLoader
    {
        public IRunner? GetRunner(string name);
    }
}
