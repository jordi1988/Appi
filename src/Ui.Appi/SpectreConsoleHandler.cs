using Core.Abstractions;
using Core.Helper;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Spectre.Console;
using System.Data;
using Color = Spectre.Console.Color;
using Rule = Spectre.Console.Rule;

namespace Ui.Appi
{
    /// <summary>
    /// Represents the Spectre console handler.
    /// </summary>
    /// <seealso cref="Core.Abstractions.IHandler" />
    internal class SpectreConsoleHandler : IHandler
    {
        private const string _ruleStyle = "red dim";

        private readonly IStringLocalizer<UILayerLocalization> _localizer;
        private readonly IConfiguration _configuration;
        private readonly Preferences _options;

        public SpectreConsoleHandler(
            IStringLocalizer<UILayerLocalization> localizer,
            IOptions<Preferences> options,
            IConfiguration configuration)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public static Color ToSpectreConsoleColor(string colorName)
        {
            var color = System.Drawing.Color.FromName(colorName);

            return new Color(color.R, color.G, color.B);
        }

        /// <inheritdoc cref="IHandler.PrintSources(IEnumerable{ISource})" />
        public void PrintSources(IEnumerable<ISource> sources)
        {
            var table = new Table();
            table.Border(TableBorder.DoubleEdge);

            table.AddColumn(_localizer["Name"]);
            table.AddColumn(_localizer["Description"]);
            table.AddColumn(_localizer["Active"], config => config.Centered());
            table.AddColumn(_localizer["Source alias"] + " [i](-s / --source)[/]");
            table.AddColumn(_localizer["Group aliases"] + " [i](-g / --group)[/]");

            foreach (var source in sources)
            {
                string styleBegin = source.IsActive ? string.Empty : "[strikethrough]";
                string styleEnd = source.IsActive ? string.Empty : "[/]";

                table.AddRow(
                    $"{styleBegin}[bold]{source.Name}[/]{styleEnd}",
                    $"{styleBegin}{source.Description}{styleEnd}",
                    source.IsActive ? "X" : string.Empty,
                    $"{source.Alias}",
                    $"{string.Join(", ", source.Groups ?? Array.Empty<string>())}"
                );
            }

            AnsiConsole.Write(table);
        }

        /// <inheritdoc cref="IHandler.PrintResults(IEnumerable{PromptGroup})" />
        public void PrintResults(IEnumerable<PromptGroup> results)
        {
            ClearScreen();
            DisplayBreakdownChart(results);
            var selectedItem = PromtForItemSelection(results, _localizer);
            DisplayItem(selectedItem);
            PromtForActionInvokation(selectedItem, _localizer);
        }

        /// <inheritdoc cref="IHandler.ClearScreen" />
        public void ClearScreen()
        {
            AnsiConsole.Clear();
        }

        private static ResultItemBase? PromtForItemSelection(IEnumerable<PromptGroup> items, IStringLocalizer<UILayerLocalization> localizer)
        {
            var itemsWithContent = items.Where(x => x.Items.Any());
            if (!itemsWithContent.Any())
            {
                AnsiConsole.Write(new Markup(localizer["Sorry, [bold]no items[/] were found."]));
                return null;
            }

            var rule = new Rule(localizer["[white]Please select item[/]"]);
            rule.RuleStyle(_ruleStyle);
            AnsiConsole.Write(rule);

            var prompt = new SelectionPrompt<ResultItemBase>()
                .PageSize(35)
                .HighlightStyle(new Style(Color.White, Color.DarkRed))
                .MoreChoicesText(localizer["[grey](Move up and down to reveal more items)[/]"]);

            prompt.DisabledStyle = new Style(
                background: Color.Silver,
                foreground: Color.Black,
                decoration: Decoration.Bold | Decoration.Italic);

            foreach (var group in itemsWithContent)
            {
                var groupHeader = new GroupHeaderResult(group.Name, group.Description);
                prompt.AddChoiceGroup(groupHeader, group.Items);
            }

            return AnsiConsole.Prompt(prompt);
        }

        private static void PromtForActionInvokation(ResultItemBase? item, IStringLocalizer<UILayerLocalization> localizer)
        {
            if (item is null)
            {
                return;
            }

            var actions = item.GetActions();
            if (!actions.Any())
            {
                AnsiConsole.Write(new Markup(localizer["Sorry, there is [bold]no action[/] you can choose from. [red]Goodbye.[/]"]));
                return;
            }

            var selectedAction = AnsiConsole.Prompt(
                new SelectionPrompt<ActionItem>()
                    .Title(localizer["[b]Which [red]action[/] should be invoked? [/]"])
                    .PageSize(30)
                    .HighlightStyle(new Style(Color.White, Color.DarkRed))
                    .MoreChoicesText(localizer["[grey](Move up and down to reveal more items)[/]"])
                    .AddChoices(actions));

            selectedAction.Action?.Invoke();
        }

        private static void DisplayItem(ResultItemBase? item)
        {
            if (item is null)
            {
                return;
            }

            var table = new Table();
            table.Border(TableBorder.DoubleEdge);
            table.HideHeaders();

            table.AddColumn(string.Empty);
            table.AddColumn(string.Empty);

            var properties = ReflectionHelper.GetProperties(item);
            foreach (var property in properties)
            {
                table.AddRow($"[bold]{property.Key}[/]", $"{property.Value}");
            }

            AnsiConsole.Write(table);
        }

        private void DisplayBreakdownChart(IEnumerable<PromptGroup> allResults)
        {
            if (!_options.Legend.Visible)
            {
                return;
            }

            var chartDisplayedResults = allResults
                .Where(x => x.Items.Any())
                .ToList();
            if (!chartDisplayedResults.Any())
            {
                return;
            }

            var chart = new BreakdownChart();
            var totalCount = chartDisplayedResults.Count;
            for (int i = 0; i < totalCount; i++)
            {
                var group = chartDisplayedResults[i];
                var currentColor = CalculateColor(totalCount, i);

                chart.AddItem(
                    group.Name,
                    group.Items.Count(),
                    currentColor);
            }

            var upperRule = new Rule();
            upperRule.RuleStyle(_ruleStyle);
            AnsiConsole.Write(upperRule);

            AnsiConsole.Write(chart);
            AnsiConsole.WriteLine();
        }

        private Color CalculateColor(int totalCount, int i)
        {
            var chartColorNames = _options.Legend.SourceColors;

            string outputColorName;
            if (i < totalCount)
            {
                outputColorName = chartColorNames[i];
            }
            else
            {
                outputColorName = chartColorNames[i - totalCount];
            }

            return ToSpectreConsoleColor(outputColorName);
        }
    }
}
