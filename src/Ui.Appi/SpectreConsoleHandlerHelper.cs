using Core.Abstractions;
using Core.Models;
using Spectre.Console;

namespace Ui.Appi
{
    public class SpectreConsoleHandlerHelper : IHandlerHelper
    {
        private readonly IHandler _handler;

        public SpectreConsoleHandlerHelper(IHandler handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public string EscapeMarkup(string? text)
        {
            return text?.EscapeMarkup() ?? string.Empty;
        }

        public string RemoveMarkup(string? text)
        {
            return text?.RemoveMarkup() ?? string.Empty;
        }

        public ActionItem Back()
        {
            return new()
            {
                Name = "Back",
                Action = () =>
                {
                    _handler.ClearScreen();

                    var allResults = _handler.ReadResultsFromMemory();

                    _handler.CreateBreakdownChart(allResults);
                    var selectedItem = _handler.PromtForItemSelection(allResults);
                    _handler.DisplayItem(selectedItem);
                    _handler.PromtForActionInvokation(selectedItem);
                }
            };
        }

        public ActionItem Exit()
        {
            return new()
            {
                Name = "Exit",
                Action = () =>
                {
                    AnsiConsole.Write(new Markup("[red]Goodbye.[/]"));
                    Environment.Exit(0);
                }
            };
        }
    }
}
