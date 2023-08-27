using Core.Abstractions;
using Core.Extensions;
using Core.Models;
using Infrastructure.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    public sealed partial class FindItemsCommand : Command<FindItemsCommand.Settings>
    {
        private readonly IHandler _handler;
        private readonly FileSettingsService _sourceService;

        public FindItemsCommand(IHandler handler, FileSettingsService sourceService)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
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
                    var options = Settings.ToOptions(settings);
                    var strategy = _sourceService.CalculateStrategy(options, Settings.QueryAllDefaultValue);
                    var sources = strategy
                        .GetSources()
                        .OrderBy(x => x.SortOrder);

                    var collectingDataTask = ctx.AddTask(
                        $"Collecting data with query `{settings.Query}` within {strategy.QueryWithinDescription}",
                        true,
                        sources.Count());

                    foreach (var source in sources)
                    {
                        var sourceResults = source
                            .ReadAsync(options)
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

            _handler.CreateBreakdownChart(allResults);
            var selectedItem = _handler.PromtForItemSelection(allResults);
            _handler.DisplayItem(selectedItem);
            _handler.PromtForActionInvokation(selectedItem);

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
