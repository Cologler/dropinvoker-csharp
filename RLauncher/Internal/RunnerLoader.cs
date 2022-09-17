using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class RunnerLoader : IRunnerLoader
    {
        private readonly IEnumerable<IRunnerPathEnumerator> _pathEnumerators;
        private readonly IEnumerable<IDocumentLoader<IRunnerData>> _dataLoaders;

        public RunnerLoader(IEnumerable<IRunnerPathEnumerator> pathEnumerators, IEnumerable<IDocumentLoader<IRunnerData>> dataLoaders)
        {
            this._pathEnumerators = pathEnumerators;
            this._dataLoaders = dataLoaders;
        }

        public async ValueTask<IRunner?> GetRunnerAsync(string name)
        {
            foreach (var pathEnumerator in this._pathEnumerators)
            {
                await foreach (var path in pathEnumerator.EnumeratePathsAsync(name).ConfigureAwait(false))
                {
                    if (File.Exists(path))
                    {
                        foreach (var dataLoader in this._dataLoaders)
                        {
                            if (await dataLoader.CanLoadAsync(path).ConfigureAwait(false))
                            {
                                var data = await dataLoader.LoadAsync(path).ConfigureAwait(false);
                                var runner = new Runner(data);
                                return runner;
                            }
                        }
                    }
                }
            }

            return default;
        }
    }
}
