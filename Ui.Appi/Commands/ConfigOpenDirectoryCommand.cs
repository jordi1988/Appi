using Spectre.Console.Cli;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Ui.Appi.Helper;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigOpenDirectoryCommand : Command<ConfigOpenDirectoryCommand.Settings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            Process.Start("explorer.exe", ConfigurationHelper.ApplicationDirectory);
            return 0;
        }

        public sealed class Settings : CommandSettings
        {
        }
    }
}
