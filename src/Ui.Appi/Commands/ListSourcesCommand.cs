using Core.Abstractions;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    public sealed partial class ListSourcesCommand : Command
    {
        private readonly IHandler _handler;
        private readonly ISettingsService _settingsService;

        public ListSourcesCommand(IHandler handler, ISettingsService sourceService)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _settingsService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
        }

        public override int Execute([NotNull] CommandContext context)
        {
            var sources = _settingsService.ReadSettingsFileSources();

            _handler.PrintSources(sources);

            return 0;
        }
    }
}
