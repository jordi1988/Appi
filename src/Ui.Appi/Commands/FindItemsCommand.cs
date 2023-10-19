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
    /// <summary>
    /// Represents the main command handling the search query and the outcome.
    /// </summary>
    internal sealed partial class FindItemsCommand : AsyncCommand<FindItemsCommand.Settings>
    {
        private readonly IHandler _handler;
        private readonly IResultStateService<PromptGroup> _resultState;
        private readonly QueryStrategyCalculator _strategyCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindItemsCommand"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="resultState">State of the result.</param>
        /// <param name="strategyCalculator">The strategy calculator.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FindItemsCommand(IHandler handler, IResultStateService<PromptGroup> resultState, QueryStrategyCalculator strategyCalculator)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _resultState = resultState ?? throw new ArgumentNullException(nameof(resultState));
            _strategyCalculator = strategyCalculator ?? throw new ArgumentNullException(nameof(strategyCalculator));
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
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

        /// <summary>
        /// Represents the settings class associated with the command.
        /// </summary>
        /// <seealso cref="CommandSettings" />
        public sealed class Settings : CommandSettings
        {
            /// <summary>
            /// The default value for the query-all search term.
            /// </summary>
            public const string QueryAllDefaultValue = "all";

            /// <summary>
            /// The search term.
            /// </summary>
            /// <value>The query.</value>
            [Description("Search for the given query.")]
            [CommandArgument(0, "<query>")]
            public string Query { get; init; } = string.Empty;

            /// <summary>
            /// The group alias for searching within a specific group.
            /// </summary>
            /// <value>
            /// The group alias.
            /// </value>
            /// <remarks>Defaults to all sources.</remarks>
            [Description("Search within a group.")]
            [CommandOption("-g|--group")]
            [DefaultValue(QueryAllDefaultValue)]
            public string? GroupAlias { get; init; }

            /// <summary>
            /// The source alias for searching within a specific source.
            /// </summary>
            /// <value>
            /// The source alias.
            /// </value>
            [Description("Search within a single source.")]
            [CommandOption("-s|--source")]
            public string? SourceAlias { get; init; }

            /// <summary>
            /// Defines if the search term should be handled case sensitive.
            /// </summary>
            /// <value>
            ///   <c>true</c> if case sensitive; otherwise, <c>false</c>.
            /// </value>
            [Description("The query parameter will be case-sensitive.")]
            [CommandOption("-c|--case-sensitive")]
            [DefaultValue(false)]
            public bool CaseSensitive { get; init; }

            /// <summary>
            /// Validates the provided settings.
            /// </summary>
            /// <returns></returns>
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
