using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Ui.Appi.Helper;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigAllowLibrariesCommand : Command<ConfigAllowLibrariesCommand.Settings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            if (settings.IsAllowed)
            {
                RegistryHelper.AllowExternalLibraries();
            }
            else
            {
                RegistryHelper.DisallowExternalLibraries();
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
