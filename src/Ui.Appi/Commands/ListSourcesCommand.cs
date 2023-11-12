using Core.Abstractions;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    /// <summary>
    /// Represents the command to list all registered sources.
    /// </summary>
    internal sealed partial class ListSourcesCommand : Command
    {
        private readonly IHandler _handler;
        private readonly ISourceService _settingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSourcesCommand"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="sourceService">The source service.</param>
        /// <exception cref="ArgumentNullException"> </exception>
        public ListSourcesCommand(IHandler handler, ISourceService sourceService)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _settingsService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The context.</param>
        public override int Execute([NotNull] CommandContext context)
        {
            var sources = _settingsService.ReadSources();

            _handler.ListSources(sources);

            return 0;
        }
    }
}
