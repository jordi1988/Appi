﻿using Domain.Entities;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Ui.Appi.Handler;
using Ui.Appi.Helper;

namespace Ui.Appi.Commands
{
    internal sealed class FindItemsCommand : Command<FindItemsCommand.Settings>
    {
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
                        .GetActiveSources(settings)
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

            var handler = new SpectreConsoleHandler();
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

            [CommandOption("-c|--case-sensitive")]
            [DefaultValue(false)]
            public bool CaseSensitive { get; init; }
        }
    }
}