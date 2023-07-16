using Spectre.Console.Cli;
using System.ComponentModel;

namespace Ui.Appi.Commands
{
    public sealed partial class FindItemsCommand
    {
        public sealed class Settings : CommandSettings
        {
            [Description("Search for the given query in all active sources.")]
            [CommandArgument(0, "<query>")]
            public string Query { get; init; } = string.Empty;

            [Description("The query parameter will be case-sensitive.")]
            [CommandOption("-c|--case-sensitive")]
            [DefaultValue(false)]
            public bool CaseSensitive { get; init; }
        }
    }
}
