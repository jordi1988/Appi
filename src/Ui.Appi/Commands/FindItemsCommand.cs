﻿using Core.Abstractions;
using Core.Extensions;
using Core.Models;
using Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    public sealed partial class FindItemsCommand : Command<FindItemsCommand.Settings>
    {
        private readonly IPluginService _pluginService;
        private readonly SourceService _sourceService;

        public FindItemsCommand(IPluginService pluginService, SourceService sourceService)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
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
                    var sources = _sourceService
                        .GetActiveSources(_pluginService)
                        .OrderBy(x => x.SortOrder);

                    // TODO: Fetch and append sources separately from the service
                    // sources = sources.Union(_externalLibraryService.GetActiveSources());

                    var collectingDataTask = ctx.AddTask("Collecting data",
                        true,
                        sources.Count());

                    foreach (var source in sources)
                    {
                        var options = Settings.ToOptions(settings);
                        var sourceResults = source.ReadAsync(options)
                            .GetAwaiter()
                            .GetResult()
                            .SortResults();

                        allResults.Add(new PromptGroup()
                        {
                            Name = source.Name,
                            Description = source.Description,
                            Items = sourceResults
                        });

                        collectingDataTask.Increment(1);
                    }

                    collectingDataTask.StopTask();
                });

            var handler = new SpectreConsoleHandler();

            handler.CreateBreakdownChart(allResults);
            var selectedItem = handler.PromtForItemSelection(allResults);
            handler.DisplayItem(selectedItem);
            handler.PromtForActionInvokation(selectedItem);

            return 0;
        }

        public sealed class Settings : CommandSettings
        {
            [Description("Search for the given query in all active sources.")]
            [CommandArgument(0, "<query>")]
            public string Query { get; init; } = string.Empty;

            [Description("The query parameter will be case-sensitive.")]
            [CommandOption("-c|--case-sensitive")]
            [DefaultValue(false)]
            public bool CaseSensitive { get; init; }

            internal static FindItemsOptions ToOptions(Settings settings) => new()
            {
                Query = settings.Query,
                CaseSensitive = settings.CaseSensitive,
            };
        }
    }
}