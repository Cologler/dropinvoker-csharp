using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;

namespace RLauncher.Internal
{
    class CommandLoader(IServiceProvider serviceProvider,
        IEnumerable<ICommandPathEnumerator> pathEnumerators, IEnumerable<IDocumentLoader<ICommandData>> dataLoaders) : ICommandLoader
    {
        private readonly ObjectFactory _commandFactory = ActivatorUtilities.CreateFactory(typeof(Command), [typeof(ICommandData)]);

        public async ValueTask<ICommand?> GetCommandAsync(string name)
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
                                return (Command)this._commandFactory(serviceProvider, [data]);
                            }
                        }
                    }
                }
            }

            return default;
        }
    }
}
