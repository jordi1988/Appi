using Domain.Interfaces;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigAllowLibrariesCommand : Command<ConfigAllowLibrariesCommand.Settings>
    {
        private readonly IExternalLibraryService _externalLibraryService;

        public ConfigAllowLibrariesCommand(IExternalLibraryService externalLibraryService)
        {
            _externalLibraryService = externalLibraryService ?? throw new ArgumentNullException(nameof(externalLibraryService));
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            if (settings.IsAllowed)
            {
                _externalLibraryService.Allow();
            }
            else
            {
                _externalLibraryService.Prohibit();
            }

            return 0;
        }

        public sealed class Settings : CommandSettings
        {
            [Description("Wether to allow external dependencies or not.")]
            [CommandArgument(0, "<allowed: true|false>")]
            public bool IsAllowed { get; init; }
        }
    }
}
