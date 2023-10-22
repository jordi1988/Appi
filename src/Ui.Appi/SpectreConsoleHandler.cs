using Core.Abstractions;
using Core.Helper;
using Core.Models;
using Microsoft.Extensions.Localization;
using Spectre.Console;
using System.Data;
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
        private readonly Color[] _chartColors = {
            Color.SkyBlue2,
            Color.Magenta2_1,
            Color.DarkRed_1,
            Color.IndianRed,
            Color.LightGoldenrod2,
            Color.LightGreen,
            Color.Blue3,
            Color.LightPink1,
            Color.LightSeaGreen,
            Color.NavajoWhite3,
            Color.Olive,
            Color.GreenYellow,
            Color.Orange4_1,
            Color.PaleGreen3,
            Color.SandyBrown
        };

        public SpectreConsoleHandler(IStringLocalizer<UILayerLocalization> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
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
            Color output;
            if (i < totalCount)
            {
                output = _chartColors[i];
            }
            else
            {
                output = _chartColors[i - totalCount];
            }

            return output;
        }
    }
}
