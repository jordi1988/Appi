using Core.Abstractions;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigAllowLibrariesCommand : Command<ConfigAllowLibrariesCommand.Settings>
    {
        private readonly IPluginService _pluginService;

        public ConfigAllowLibrariesCommand(IPluginService pluginService)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            if (settings.IsAllowed)
            {
                _pluginService.Allow();
            }
            else
            {
                _pluginService.Prohibit();
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
