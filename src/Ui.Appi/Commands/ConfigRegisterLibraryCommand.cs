﻿using Core.Abstractions;
using Core.Helper;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigRegisterLibraryCommand : Command<ConfigRegisterLibraryCommand.Settings>
    {
        private readonly IPluginService _pluginService;
        private readonly ISettingsService _settingsService;

        public ConfigRegisterLibraryCommand(IPluginService pluginService, ISettingsService sourceService)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _settingsService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            ConfigurationHelper.EnsureSettingsExist();

            var filename = Path.GetFileName(settings.Path);

            if (!File.Exists(settings.Path))
            {
                AnsiConsole.Write(
                    new Markup($"[yellow]The file [gray]{filename}[/] could not be found.[/]"));
                return 1;
            }

            if (!Path.GetExtension(settings.Path).Equals(".dll", StringComparison.CurrentCultureIgnoreCase))
            {
                AnsiConsole.Write(
                    new Markup($"[yellow]The file [gray]{filename}[/] must be of format DLL.[/]"));
                return 1;
            }

            var newFilePath = Path.Combine(ConfigurationHelper.ApplicationDirectory, filename);
            CopyToConfigDirectory(settings, newFilePath);
            AppendToConfigFile(settings, newFilePath);

            AnsiConsole.Write(
                new Markup($"The file [gray]{filename}[/] was installed."));

            if (!_pluginService.IsAllowed())
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(
                    new Markup("[yellow]Remember to allow external plugins to be loaded using the `config` command.[/]"));
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
            var commandSettings = new FindItemsCommand.Settings();

            foreach (var classType in classTypes)
            {
                var sourceInstance = ReflectionHelper.CreateInstance<ISource>(classType, commandSettings);
                var currentSettings = _settingsService
                    .ReadSettingsFileSources()
                    .ToList();

                currentSettings.Add(sourceInstance);

                _settingsService.SaveSettingsFileSources(currentSettings);
            }
        }

        public sealed class Settings : CommandSettings
        {
            [Description("Path to the DLL file.")]
            [CommandArgument(0, "<path>")]
            public string Path { get; init; } = string.Empty;

            [Description("Just copy the assembly into the application's directory (e. g. for plugin updates). This won't register the assembly in `sources.json`.")]
            [CommandOption("--copy-only")]
            [DefaultValue(false)]
            public bool CopyOnly { get; init; }
        }
    }
}
