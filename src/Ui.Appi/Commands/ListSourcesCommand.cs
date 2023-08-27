using Core.Abstractions;
using Infrastructure.Services;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    public sealed partial class ListSourcesCommand : Command
    {
        private readonly IHandler _handler;
        private readonly FileSettingsService _sourceService;

        public ListSourcesCommand(IHandler handler, FileSettingsService sourceService)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
        }

        public override int Execute([NotNull] CommandContext context)
        {
            var sources = _sourceService.ReadSettingsFileSources();

            _handler.DisplaySources(sources);

            return 0;
        }
    }
}
