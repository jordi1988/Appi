using Core.Helper;
using Infrastructure.Services;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    /// <summary>
    /// Represents the command of openening the config directory.
    /// </summary>
    internal sealed class ConfigOpenDirectoryCommand : Command
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The context.</param>
        public override int Execute([NotNull] CommandContext context)
        {
            ProcessService.Start("explorer.exe", ConfigurationHelper.ApplicationDirectory);

            return 0;
        }
    }
}
