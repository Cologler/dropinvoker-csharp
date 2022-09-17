using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class CommandLoader : ICommandLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<ICommandPathEnumerator> _pathEnumerators;
        private readonly IEnumerable<IDocumentLoader<ICommandData>> _dataLoaders;
        private readonly ObjectFactory _commandFactory;

        public CommandLoader(IServiceProvider serviceProvider,
            IEnumerable<ICommandPathEnumerator> pathEnumerators, IEnumerable<IDocumentLoader<ICommandData>> dataLoaders)
        {
            this._serviceProvider = serviceProvider;
            this._pathEnumerators = pathEnumerators;
            this._dataLoaders = dataLoaders;
            this._commandFactory = ActivatorUtilities.CreateFactory(typeof(Command), new[] { typeof(ICommandData) });
        }

        public async ValueTask<ICommand?> GetCommandAsync(string name)
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
                                return (Command)this._commandFactory(this._serviceProvider, new object[] { data });
                            }
                        }
                    }
                }
            }

            return default;
        }
    }
}
