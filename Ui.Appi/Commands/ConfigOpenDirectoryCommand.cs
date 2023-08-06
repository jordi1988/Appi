using Core.Helper;
using Infrastructure.Services;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    internal sealed class ConfigOpenDirectoryCommand : Command
    {
        public override int Execute([NotNull] CommandContext context)
        {
            ProcessService.Start("explorer.exe", ConfigurationHelper.ApplicationDirectory);

            return 0;
        }
    }
}
