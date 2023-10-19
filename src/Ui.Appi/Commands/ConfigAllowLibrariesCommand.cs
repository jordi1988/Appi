using Core.Abstractions;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    /// <summary>
    /// Represents the command of allowing or prohibiting custom plugins to be run.
    /// </summary>
    internal sealed class ConfigAllowLibrariesCommand : Command<ConfigAllowLibrariesCommand.Settings>
    {
        private readonly IPluginService _pluginService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigAllowLibrariesCommand"/> class.
        /// </summary>
        /// <param name="pluginService">The plugin service.</param>
        /// <exception cref="ArgumentNullException">pluginService</exception>
        public ConfigAllowLibrariesCommand(IPluginService pluginService)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
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

        /// <summary>
        /// Represents the settings class associated with the command.
        /// </summary>
        /// <seealso cref="CommandSettings" />
        public sealed class Settings : CommandSettings
        {
            [Description("Wether to allow external dependencies or not.")]
            [CommandArgument(0, "<allowed: true|false>")]
            public bool IsAllowed { get; init; }
        }
    }
}
