using Core.Abstractions;
using Core.Helper;
using Microsoft.Extensions.Localization;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Ui.Appi.Commands
{
    /// <summary>
    /// Represents the command to register new or updated custom plugins.
    /// </summary>
    internal sealed class ConfigRegisterLibraryCommand : Command<ConfigRegisterLibraryCommand.Settings>
    {
        private readonly IPluginService _pluginService;
        private readonly ISettingsService _settingsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<UILayerLocalization> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigRegisterLibraryCommand"/> class.
        /// </summary>
        /// <param name="pluginService">The plugin service.</param>
        /// <param name="sourceService">The source service.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <param name="localizer">The string localizer.</param>
        /// <exception cref="ArgumentNullException"> </exception>
        public ConfigRegisterLibraryCommand(
            IPluginService pluginService, 
            ISettingsService sourceService,
            IServiceProvider serviceProvider,
            IStringLocalizer<UILayerLocalization> localizer)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _settingsService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _localizer = localizer;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            ConfigurationHelper.EnsureSettingsExist();

            var filename = Path.GetFileName(settings.Path);

            if (!File.Exists(settings.Path))
            {
                AnsiConsole.Write(
                    new Markup(_localizer["[yellow]The file [gray]{0}[/] could not be found.[/]", filename]));
                return 1;
            }

            if (!Path.GetExtension(settings.Path).Equals(".dll", StringComparison.CurrentCultureIgnoreCase))
            {
                AnsiConsole.Write(
                    new Markup(_localizer["[yellow]The file [gray]{0}[/] must be of format DLL.[/]", filename]));
                return 1;
            }

            var newFilePath = Path.Combine(ConfigurationHelper.ApplicationDirectory, filename);
            CopyToConfigDirectory(settings, newFilePath);
            AppendToConfigFile(settings, newFilePath);

            AnsiConsole.Write(
                new Markup(_localizer["The file [gray]{0}[/] was installed.", filename]));

            if (!_pluginService.IsAllowed())
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(
                    new Markup(_localizer["[yellow]Remember to allow external plugins to be loaded using the 'config' command.[/]"]));
            }

            return 0;
        }

        private static void CopyToConfigDirectory(Settings settings, string newFilePath)
        {
            File.Copy(settings.Path, newFilePath, true);
        }

        private void AppendToConfigFile(Settings settings, string newFilePath)
        {
            if (settings.CopyOnly)
            {
                return;
            }

            var newAssembly = Assembly.UnsafeLoadFrom(newFilePath);
            var classTypes = ReflectionHelper.GetClassesImplementingInterface<ISource>(newAssembly);

            foreach (var classType in classTypes)
            {
                var sourceInstance = ReflectionHelper.CreateInstance<ISource>(classType, _serviceProvider);
                var currentSettings = _settingsService
                    .ReadSources()
                    .ToList();

                currentSettings.Add(sourceInstance);

                _settingsService.SaveSources(currentSettings);
            }
        }

        /// <summary>
        /// Represents the settings class associated with the command.
        /// </summary>
        /// <seealso cref="CommandSettings" />
        public sealed class Settings : CommandSettings
        {
            /// <summary>
            /// The path to the plugin file.
            /// </summary>
            /// <value>The path.</value>
            [Description("Path to the DLL file.")]
            [CommandArgument(0, "<path>")]
            public string Path { get; init; } = string.Empty;

            /// <summary>
            /// Defines if the file should only be copied, but not registered in the config.
            /// </summary>
            /// <value>
            ///   <c>true</c> if copy only; otherwise register in config file.
            /// </value>
            [Description("Just copy the assembly into the application's directory (e. g. for plugin updates). This won't register the assembly in `sources.json`.")]
            [CommandOption("--copy-only")]
            [DefaultValue(false)]
            public bool CopyOnly { get; init; }
        }
    }
}
