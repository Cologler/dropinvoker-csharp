namespace RLauncher.Internal
{
    /// <summary>
    /// a <see cref="IRunnerLoader"/> with always return <see langword="null"/>.
    /// </summary>
    internal class NullRunnerLoader : IRunnerLoader
    {
        public IRunner? GetRunner(string name) => null;
    }
}
