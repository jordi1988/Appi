using Core.Abstractions;
using Core.Extensions;
using Core.Models;
using Core.Strategies;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    public sealed partial class FindItemsCommand : AsyncCommand<FindItemsCommand.Settings> // Command<FindItemsCommand.Settings>
    {
        private readonly IHandler _handler;
        private readonly IResultStateService _resultState;
        private readonly QueryStrategyCalculator _strategyCalculator;

        public FindItemsCommand(IHandler handler, IResultStateService resultState, QueryStrategyCalculator strategyCalculator)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _resultState = resultState ?? throw new ArgumentNullException(nameof(resultState));
            _strategyCalculator = strategyCalculator ?? throw new ArgumentNullException(nameof(strategyCalculator));
        }

        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            PromptGroup[] results = Array.Empty<PromptGroup>();

            await AnsiConsole.Progress()
                .AutoClear(true)
                .Columns(
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new SpinnerColumn()
                )
                .StartAsync(async ctx =>
                {
                    var options = Settings.ToOptions(settings);
                    var strategy = _strategyCalculator.Create(options, Settings.QueryAllDefaultValue);
                    var sources = strategy
                        .GetSources()
                        .OrderBy(x => x.SortOrder);

                    var collectingDataTask = ctx.AddTask(
                        $"Collecting data with query `{settings.Query}` within {strategy.QueryWithinDescription}",
                        true,
                        sources.Count());

                    while (!ctx.IsFinished)
                    {
                        var sourceTasks = sources.Select(async source =>
                        {
                            var sourceResults = await source.ReadAsync(options);
                            sourceResults.SortResults();

                            collectingDataTask.Increment(1);

                            return new PromptGroup(
                                source.Name,
                                source.Description,
                                sourceResults);
                        });

                        results = await Task.WhenAll(sourceTasks);

                        collectingDataTask.StopTask();
                    }
                });

            _resultState.Save(results);
            _handler.PrintResults(results);

            return 0;
        }

        public sealed class Settings : CommandSettings
        {
            public const string QueryAllDefaultValue = "all";

            [Description("Search for the given query.")]
            [CommandArgument(0, "<query>")]
            public string Query { get; init; } = string.Empty;

            [Description("Search within a group.")]
            [CommandOption("-g|--group")]
            [DefaultValue(QueryAllDefaultValue)]
            public string? GroupAlias { get; init; }

            [Description("Search within a single source.")]
            [CommandOption("-s|--source")]
            public string? SourceAlias { get; init; }

            [Description("The query parameter will be case-sensitive.")]
            [CommandOption("-c|--case-sensitive")]
            [DefaultValue(false)]
            public bool CaseSensitive { get; init; }

            public override ValidationResult Validate()
            {
                bool groupAliasEmptyOrDefault =
                    string.IsNullOrWhiteSpace(GroupAlias) ^
                    QueryAllDefaultValue.Equals(GroupAlias);

                bool sourceAndGroupBothProvided =
                    !string.IsNullOrWhiteSpace(SourceAlias) &&
                    !groupAliasEmptyOrDefault;

                if (sourceAndGroupBothProvided)
                {
                    return ValidationResult.Error("You can only pass one option, either `source` or `group`.");
                }

                return base.Validate();
            }

            internal static FindItemsOptions ToOptions(Settings settings) => new()
            {
                Query = settings.Query,
                GroupAlias = settings.GroupAlias,
                SourceAlias = settings.SourceAlias,
                CaseSensitive = settings.CaseSensitive,
            };
        }
    }
}
