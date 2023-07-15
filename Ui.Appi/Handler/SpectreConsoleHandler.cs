using Domain.Entities;
using Domain.Interfaces;
using Spectre.Console;

namespace Ui.Appi.Handler
{
    internal class SpectreConsoleHandler : IHandler
    {
        public Result PromtForItemSelection(IEnumerable<PromptGroup> items)
        {
            var prompt = new SelectionPrompt<Result>()
                .Title("[b]Please [red]select item[/][/]:")
                .PageSize(20)
                .HighlightStyle(new Style(Color.White, Color.DarkRed))
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]");

            var itemsWithContent = items.Where(x => x.Items.Any());
            foreach (var group in itemsWithContent)
            {
                prompt.AddChoiceGroup(new GroupHeaderResult(group.Name, group.Description), group.Items);
            }

            return AnsiConsole.Prompt(prompt);
        }

        public void PromtForActionInvokation(Result item)
        {
            var actions = item.GetActions();
            if (!actions.Any())
            {
                AnsiConsole.Write(new Markup("Sorry, there is [bold]no action[/] you can choose from. [red]Goodbye![/]"));
                return;
            }

            var selectedAction = AnsiConsole.Prompt(
                new SelectionPrompt<ActionItem>()
                    .Title("[b]Which [red]action[/] should be invoked?[/]:")
                    .PageSize(50)
                    .HighlightStyle(new Style(Color.White, Color.DarkRed))
                    .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                    .AddChoices(actions));

            selectedAction.Action?.Invoke();
        }

        // TODO: this is not ideal: displayed output may differ among selected results
        public void DisplayItem(Result item)
        {
            var grid = new Grid();

            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow("[bold]ID[/]", item.Id.ToString());
            grid.AddRow("[bold]Name[/]", item.Name);
            grid.AddRow("[bold]Description[/]", item.Description);

            AnsiConsole.Write(grid);
        }
    }
}
