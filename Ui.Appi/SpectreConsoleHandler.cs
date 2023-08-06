using Core.Abstractions;
using Core.Entities;
using Core.Helper;
using Spectre.Console;

namespace Ui.Appi
{
    internal class SpectreConsoleHandler : IHandler
    {
        public ResultItemBase PromtForItemSelection(IEnumerable<PromptGroup> items)
        {
            var prompt = new SelectionPrompt<ResultItemBase>()
                .Title("[b]Please [red]select item[/][/]:")
                .PageSize(30)
                .HighlightStyle(new Style(Color.White, Color.DarkRed))
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]");

            var itemsWithContent = items.Where(x => x.Items.Any());
            foreach (var group in itemsWithContent)
            {
                prompt.AddChoiceGroup(new GroupHeaderResult(group.Name, group.Description), group.Items);
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
    }
}
