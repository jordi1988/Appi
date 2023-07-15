using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigAddSourceCommand : Command<ConfigAddSourceCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("Path to search. Defaults to current directory.")]
            [CommandArgument(0, "<name>")]
            public string Name { get; init; } = string.Empty;

            [CommandOption("-p|--pattern")]
            public string? SearchPattern { get; init; }

            [CommandOption("--hidden")]
            [DefaultValue(true)]
            public bool IncludeHidden { get; init; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var searchOptions = new EnumerationOptions
            {
                AttributesToSkip = settings.IncludeHidden
                    ? FileAttributes.Hidden | FileAttributes.System
                    : FileAttributes.System
            };

            var searchPattern = settings.SearchPattern ?? "*.*";
            var searchPath = settings.Name ?? Directory.GetCurrentDirectory();
            var files = new DirectoryInfo(searchPath)
                .GetFiles(searchPattern, searchOptions);

            var totalFileSize = files
                .Sum(fileInfo => fileInfo.Length);

            AnsiConsole.MarkupLine($"Total file size for [green]{searchPattern}[/] files in [green]{searchPath}[/]: [blue]{totalFileSize:N0}[/] bytes");

            // Console.ReadKey();

            return 0;
        }
    }
}
