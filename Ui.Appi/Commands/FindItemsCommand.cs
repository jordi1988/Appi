using Domain.Entities;
using Domain.Interfaces;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using Ui.Appi.Handler;
using Ui.Appi.Helper;

namespace Ui.Appi.Commands
{
    // TODO: sort order for items based on ...
    public sealed partial class FindItemsCommand : Command<FindItemsCommand.Settings>
    {
        private readonly IExternalLibraryService _externalLibraryService;

        public FindItemsCommand(IExternalLibraryService externalLibraryService)
        {
            _externalLibraryService = externalLibraryService ?? throw new ArgumentNullException(nameof(externalLibraryService));
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var allResults = new List<PromptGroup>();

            AnsiConsole.Progress()
                .AutoClear(true)
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn()
                )
                .Start(ctx =>
                {
                    var sources = ConfigurationHelper
                        .GetActiveSources(settings, _externalLibraryService)
                        .OrderBy(x => x.SortOrder);

                    var collectingDataTask = ctx.AddTask("Collecting data",
                        true,
                        sources.Count());

                    foreach (var source in sources)
                    {
                        var results = source.ReadAsync()
                            .GetAwaiter()
                            .GetResult();

                        allResults.Add(new PromptGroup()
                        {
                            Name = source.Name,
                            Description = source.Description,
                            Items = results
                        });

                        collectingDataTask.Increment(1);
                    }

                    collectingDataTask.StopTask();
                });

            // TODO: combine result and handler to display specific result based on IHandler
            var handler = new SpectreConsoleHandler();
            var selectedItem = handler.PromtForItemSelection(allResults);
            handler.DisplayItem(selectedItem);
            handler.PromtForActionInvokation(selectedItem);

            return 0;
        }
    }
}
