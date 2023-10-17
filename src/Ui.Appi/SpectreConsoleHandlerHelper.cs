using Core.Abstractions;
using Core.Models;
using Spectre.Console;

namespace Ui.Appi
{
    public class SpectreConsoleHandlerHelper : IHandlerHelper
    {
        private readonly IHandler _handler;
        private readonly IResultStateService<PromptGroup> _resultState;

        public SpectreConsoleHandlerHelper(IHandler handler, IResultStateService<PromptGroup> resultState)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _resultState = resultState ?? throw new ArgumentNullException(nameof(resultState));
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
                    var results = _resultState.Load();
                    _handler.PrintResults(results);
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
