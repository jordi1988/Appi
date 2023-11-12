using Core.Abstractions;
using Core.Helper;
using Core.Models;
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
        private readonly IStringLocalizer<UILayerLocalization> _localizer;
        private readonly Preferences _options;
        private Style HeighlightStyle => new(Color.White, GetSpectreConsoleColor(_options.AccentColor));

        public SpectreConsoleHandler(
            IStringLocalizer<UILayerLocalization> localizer,
            IOptions<Preferences> options)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc cref="IHandler.ListSources(IEnumerable{ISource})" />
        public void ListSources(IEnumerable<ISource> sources)
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

        private ResultItemBase? PromtForItemSelection(IEnumerable<PromptGroup> items, IStringLocalizer<UILayerLocalization> localizer)
        {
            var itemsWithContent = items.Where(x => x.Items.Any());
            if (!itemsWithContent.Any())
            {
                AnsiConsole.Write(new Markup(localizer["Sorry, [bold]no items[/] were found."]));
                return null;
            }

            var rule = new Rule(localizer["[white]Please select item[/]"]);
            rule.RuleStyle($"{_options.AccentColor} dim");
            AnsiConsole.Write(rule);

            var prompt = new SelectionPrompt<ResultItemBase>()
                .PageSize(_options.PageSize)
                .HighlightStyle(HeighlightStyle)
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

        private void PromtForActionInvokation(ResultItemBase? item, IStringLocalizer<UILayerLocalization> localizer)
        {
            if (item is null)
            {
                return;
            }

            var actions = item.GetActions();
            if (!actions.Any())
            {
                AnsiConsole.Write(new Markup($"{localizer["Sorry, there is [bold]no action[/] you can choose from."]} [{_options.AccentColor}]{_localizer["Goodbye."]}[/]"));
                return;
            }

            var selectedAction = AnsiConsole.Prompt(
                new SelectionPrompt<ActionItem>()
                    .Title(localizer["[b]Which {0} should be invoked? [/]", $"[{_options.AccentColor}]{localizer["action"]}[/]"])
                    .PageSize(10)
                    .HighlightStyle(HeighlightStyle)
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
            upperRule.RuleStyle($"{_options.AccentColor} dim");
            AnsiConsole.Write(upperRule);

            AnsiConsole.Write(chart);
            AnsiConsole.WriteLine();
        }

        private Color CalculateColor(int totalCount, int i)
        {
            var chartColorNames = _options.Legend.SourceColors!;

            string outputColorName;
            if (i < totalCount)
            {
                outputColorName = chartColorNames[i];
            }
            else
            {
                outputColorName = chartColorNames[i - totalCount];
            }

            return GetSpectreConsoleColor(outputColorName);
        }

        private static Color GetSpectreConsoleColor(string colorName)
        {
            var color = System.Drawing.Color.FromName(colorName);

            return new Color(color.R, color.G, color.B);
        }
    }
}
