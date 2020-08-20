namespace RLauncher
{
    public interface IRunnerLoader
    {
        public IRunner? TryLoad(string name);
    }
}
