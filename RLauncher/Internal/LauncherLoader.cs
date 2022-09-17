using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class LauncherLoader : ILauncherLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<ILauncherPathEnumerator> _pathEnumerators;
        private readonly IEnumerable<IDocumentLoader<ILauncherData>> _dataLoaders;

        public LauncherLoader(IServiceProvider serviceProvider,
            IEnumerable<ILauncherPathEnumerator> pathEnumerators, IEnumerable<IDocumentLoader<ILauncherData>> dataLoaders)
        {
            this._serviceProvider = serviceProvider;
            this._pathEnumerators = pathEnumerators;
            this._dataLoaders = dataLoaders;
        }

        public async ValueTask<Launcher?> GetLauncherAsync(string name)
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
                                var launcher = this._serviceProvider.GetRequiredService<Launcher>();
                                launcher.LoadFrom(data);
                                return launcher;
                            }
                        }
                    }
                }
            }

            return default;
        }
    }
}
