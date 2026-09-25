
using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;
using RLauncher.Exceptions;

namespace RLauncher.Internal
{
    class Command(IServiceProvider serviceProvider, ICommandData data) : ICommand
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        private readonly CommandDataSnapshot _commandData = new CommandDataSnapshot(data);

        public string Name => this._commandData?.Name ?? string.Empty;

        public string Description => this._commandData?.Description ?? string.Empty;

        public IReadOnlyList<string> Arguments => this._commandData?.Arguments?.Where(x => x is not null).Cast<string>().ToArray() ?? [];

        public string? WorkingDirectory => this._commandData?.WorkingDirectory;

        public IReadOnlyList<string> Accepts => this._commandData?.Accepts?.Where(x => x is not null).Cast<string>().ToArray() ?? [];

        public bool IgnoreExitCode => this._commandData.IgnoreExitCode;

        private async Task<ExecuteContext> CreateContextAsync(IEnumerable<string> arguments)
        {
            ThrowIfNull(arguments);

            var runnerName = this._commandData?.Runner;
            var runner = runnerName is null
                    ? this._serviceProvider.GetRequiredKeyedService<IRunner>(ServiceCollectionExtensions.DefaultRunnerKey)
                    : await this._serviceProvider.GetRequiredService<IRunnerLoader>().GetRunnerAsync(runnerName).ConfigureAwait(false)
                    ?? throw new MissingRunnerException(runnerName);

            return new ExecuteContext(this, runner, [.. arguments]);
        }

        public async ValueTask<IReadOnlyList<string>> GetCommandAsync(IEnumerable<string> arguments)
        {
            var context = await this.CreateContextAsync(arguments).ConfigureAwait(false);
            return context.Runner.GetCommand(context);
        }

        public async Task<int> RunAsync(IEnumerable<string> arguments)
        {
            var context = await this.CreateContextAsync(arguments).ConfigureAwait(false);
            return await context.Runner.RunAsync(context).ConfigureAwait(false);
        }
    }
}
