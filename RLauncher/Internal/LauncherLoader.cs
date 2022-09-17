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
        private readonly ObjectFactory _launcherFactory;

        public LauncherLoader(IServiceProvider serviceProvider,
            IEnumerable<ILauncherPathEnumerator> pathEnumerators, IEnumerable<IDocumentLoader<ILauncherData>> dataLoaders)
        {
            this._serviceProvider = serviceProvider;
            this._pathEnumerators = pathEnumerators;
            this._dataLoaders = dataLoaders;
            this._launcherFactory = ActivatorUtilities.CreateFactory(typeof(Launcher), new[] { typeof(ILauncherData) });
        }

        public async ValueTask<ILauncher?> GetLauncherAsync(string name)
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
                                var launcher = (Launcher) this._launcherFactory(this._serviceProvider, new object[] { data });
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
