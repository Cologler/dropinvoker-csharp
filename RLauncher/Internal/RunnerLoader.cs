using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class RunnerLoader(IEnumerable<IRunnerPathEnumerator> pathEnumerators, IEnumerable<IDocumentLoader<IRunnerData>> dataLoaders) : IRunnerLoader
    {
        public async ValueTask<IRunner?> GetRunnerAsync(string name)
        {
            foreach (var pathEnumerator in pathEnumerators)
            {
                await foreach (var path in pathEnumerator.EnumeratePathsAsync(name).ConfigureAwait(false))
                {
                    if (File.Exists(path))
                    {
                        foreach (var dataLoader in dataLoaders)
                        {
                            if (await dataLoader.CanLoadAsync(path).ConfigureAwait(false))
                            {
                                var data = await dataLoader.LoadAsync(path).ConfigureAwait(false);
                                return new Runner(data);
                            }
                        }
                    }
                }
            }

            return default;
        }
    }
}
