
using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;
using RLauncher.Exceptions;

namespace RLauncher.Internal
{
    class Command : ICommand
    {
        private readonly IServiceProvider _serviceProvider;
        private ICommandData? _commandData;

        public Command(IServiceProvider serviceProvider, ICommandData data)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _commandData = new CommandDataSnapshot(data);
        }

        public string Name => _commandData?.Name ?? string.Empty;

        public string Description => _commandData?.Description ?? string.Empty;

        public IReadOnlyList<string> Arguments => _commandData?.Arguments?.Where(x => x is not null).Cast<string>().ToArray() ?? Array.Empty<string>();

        public string? WorkingDirectory => _commandData?.WorkingDirectory;

        public IReadOnlyList<string> Accepts => _commandData?.Accepts?.Where(x => x is not null).Cast<string>().ToArray() ?? Array.Empty<string>();

        public async Task RunAsync(IEnumerable<string> arguments)
        {
            ThrowIfNull(arguments);

            var runnerName = _commandData?.Runner;
            var runner = runnerName is null
                    ? _serviceProvider.GetRequiredService<IDefaultRunner>()
                    : await _serviceProvider.GetRequiredService<IRunnerLoader>().GetRunnerAsync(runnerName).ConfigureAwait(false)
                    ?? throw new MissingRunnerException(runnerName);

            var context = new ExecuteContext(this, runner, arguments.ToArray());

            await runner.RunAsync(context).ConfigureAwait(false);
        }
    }
}
