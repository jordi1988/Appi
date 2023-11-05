using Core.Abstractions;
using Core.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace Ui.Appi
{
    /// <summary>
    /// Represents the Spectre console handler helper.
    /// </summary>
    /// <seealso cref="IHandlerHelper" />
    internal class SpectreConsoleHandlerHelper : IHandlerHelper
    {
        private readonly IHandler _handler;
        private readonly IResultStateService<PromptGroup> _resultState;
        private readonly IStringLocalizer<UILayerLocalization> _localizer;
        private readonly Preferences _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpectreConsoleHandlerHelper"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="resultState">State of the result.</param>
        /// <param name="localizer">The localizer</param>
        /// <param name="options">The preferences</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SpectreConsoleHandlerHelper(
            IHandler handler,
            IResultStateService<PromptGroup> resultState, 
            IStringLocalizer<UILayerLocalization> localizer, 
            IOptions<Preferences> options)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _resultState = resultState ?? throw new ArgumentNullException(nameof(resultState));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Escapes the Spectre markup.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The text with escaped markup.</returns>
        public string EscapeMarkup(string? text)
        {
            return text?.EscapeMarkup() ?? string.Empty;
        }

        /// <summary>
        /// Removes the Spectre markup.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The text with removed markup.</returns>
        public string RemoveMarkup(string? text)
        {
            return text?.RemoveMarkup() ?? string.Empty;
        }

        /// <summary>
        /// Provides backward navigation.
        /// </summary>
        public ActionItem Back()
        {
            return new()
            {
                Name = _localizer["Back"],
                Action = () =>
                {
                    var results = _resultState.Load();
                    _handler.PrintResults(results);
                }
            };
        }

        /// <summary>
        /// Provides exit navigation.
        /// </summary>
        public ActionItem Exit()
        {
            return new()
            {
                Name = _localizer["Exit"],
                Action = () =>
                {
                    AnsiConsole.Write(new Markup($"[{_options.AccentColor}]{_localizer["Goodbye."]}[/]"));
                    Environment.Exit(0);
                }
            };
        }
    }
}
