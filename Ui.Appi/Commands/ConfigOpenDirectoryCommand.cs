using Spectre.Console.Cli;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Ui.Appi.Helper;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigOpenDirectoryCommand : Command
    {
        public override int Execute([NotNull] CommandContext context)
        {
            Process.Start("explorer.exe", ConfigurationHelper.ApplicationDirectory);
            return 0;
        }
    }
}
