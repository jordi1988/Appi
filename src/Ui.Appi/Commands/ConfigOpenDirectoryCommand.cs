using Core.Helper;
using Core.Models;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Ui.Appi.Commands
{
    /// <summary>
    /// Represents the command of openening the config directory.
    /// </summary>
    internal sealed class ConfigOpenDirectoryCommand : Command
    {
        private readonly Preferences _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigOpenDirectoryCommand"/> class.
        /// </summary>
        public ConfigOpenDirectoryCommand(IOptions<Preferences> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context">The context.</param>
        public override int Execute([NotNull] CommandContext context)
        {
            ProcessService.Start("explorer.exe", _options.AppDataDirectory);

            return 0;
        }
    }
}
