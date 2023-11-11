
using Microsoft.Extensions.DependencyInjection;

using RLauncher.Abstractions;
using RLauncher.Exceptions;

namespace RLauncher.Internal
{
    class Command : ICommand
    {
        private readonly IServiceProvider _serviceProvider;
        private ICommandData? _launcherData;

        public Command(IServiceProvider serviceProvider, ICommandData data)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _launcherData = new CommandDataSnapshot(data);
        }

        public string Name => _launcherData?.Name ?? string.Empty;

        public string Description => _launcherData?.Description ?? string.Empty;

        public IReadOnlyList<string> Arguments => _launcherData?.Arguments?.Where(x => x is not null).Cast<string>().ToArray() ?? Array.Empty<string>();

        public string? WorkingDirectory => _launcherData?.WorkingDirectory;

        public IReadOnlyList<string> Accepts => _launcherData?.Accepts?.Where(x => x is not null).Cast<string>().ToArray() ?? Array.Empty<string>();

        public async Task RunAsync(IEnumerable<string> arguments)
        {
            ThrowIfNull(arguments);

            var runnerName = _launcherData?.Runner;
            var runner = runnerName is null
                    ? _serviceProvider.GetRequiredService<IDefaultRunner>()
                    : await _serviceProvider.GetRequiredService<IRunnerLoader>().GetRunnerAsync(runnerName).ConfigureAwait(false)
                    ?? throw new MissingRunnerException(runnerName);

            var context = new RunContext(this, runner, arguments.ToArray());

            await runner.RunAsync(context).ConfigureAwait(false);
        }
    }
}
