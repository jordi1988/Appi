﻿using Core.Abstractions;
using Core.Helper;
using Core.Models;
using Spectre.Console;
using System.Data;
using Rule = Spectre.Console.Rule;

namespace Ui.Appi
{
    internal class SpectreConsoleHandler : IHandler
    {
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

        public ResultItemBase PromtForItemSelection(IEnumerable<PromptGroup> items)
        {
            var prompt = new SelectionPrompt<ResultItemBase>()
                .PageSize(30)
                .HighlightStyle(new Style(Color.White, Color.DarkRed))
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]");

            var itemsWithContent = items.Where(x => x.Items.Any());
            foreach (var group in itemsWithContent)
            {
                var groupHeader = new GroupHeaderResult(group.Name, group.Description);
                prompt.AddChoiceGroup(groupHeader, group.Items);
            }

            return AnsiConsole.Prompt(prompt);
        }

        public void PromtForActionInvokation(ResultItemBase item)
        {
            var actions = item.GetActions();
            if (!actions.Any())
            {
                AnsiConsole.Write(new Markup("Sorry, there is [bold]no action[/] you can choose from. [red]Goodbye.[/]"));
                return;
            }

            var selectedAction = AnsiConsole.Prompt(
                new SelectionPrompt<ActionItem>()
                    .Title("[b]Which [red]action[/] should be invoked?[/]:")
                    .PageSize(30)
                    .HighlightStyle(new Style(Color.White, Color.DarkRed))
                    .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                    .AddChoices(actions));

            selectedAction.Action?.Invoke();
        }

        public void DisplayItem(ResultItemBase item)
        {
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

        public void DisplaySources(IEnumerable<ISource> sources)
        {
            var table = new Table();
            table.Border(TableBorder.DoubleEdge);

            table.AddColumn("Name");
            table.AddColumn("Description");
            table.AddColumn("Active", config => config.Centered());
            table.AddColumn("Source alias [i](-s / --source)[/]");
            table.AddColumn("Group aliases [i](-g / --group)[/]");

            foreach (var source in sources)
            {
                string styleBegin = source.IsActive ? string.Empty : "[strikethrough]";
                string styleEnd = source.IsActive ? string.Empty : "[/]";

                table.AddRow(
                    $"{styleBegin}[bold]{source.Name}[/]{styleEnd}",
                    $"{styleBegin}{source.Description}{styleEnd}",
                    source.IsActive ? "X" : string.Empty,
                    $"{source.Alias}",
                    $"{string.Join(", ", source?.Groups ?? Array.Empty<string>())}"
                );
            }

            AnsiConsole.Write(table);
        }

        public void CreateBreakdownChart(List<PromptGroup> allResults)
        {
            // TODO: colors should be definable and chart should be customizable in some way 
            var chartDisplayedResults = allResults.Where(x => x.Items.Any()).ToList();

            var chart = new BreakdownChart();
            var totalCount = chartDisplayedResults.Count;
            for (int i = 0; i < totalCount; i++)
            {
                var group = chartDisplayedResults[i];
                var currentColor = CalculateColor(totalCount, i);

                chart.AddItem(group.Name, group.Items.Count(), currentColor);
            }

            var ruleStyle = "red dim";
            var upperRule = new Rule();
            upperRule.RuleStyle(ruleStyle);
            AnsiConsole.Write(upperRule);

            AnsiConsole.Write(chart);
            AnsiConsole.WriteLine();

            var lowerRule = new Rule("[white]Please select item[/]");
            lowerRule.RuleStyle(ruleStyle);
            AnsiConsole.Write(lowerRule);
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
