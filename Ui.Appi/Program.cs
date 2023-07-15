using Spectre.Console.Cli;
using System.Diagnostics;
using Ui.Appi.Commands;
using Ui.Appi.Helper;

namespace Ui.Appi
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // TODO: sort order for items based on ...

            var app = new CommandApp<FindItemsCommand>();
            app.Configure(config =>
            {
                config.AddCommand<FindItemsCommand>("find");
                config.AddBranch("config", source =>
                {
                    source.AddCommand<ConfigOpenDirectoryCommand>("open");
                    // source.AddCommand<ConfigAddSourceCommand>("add-source");
                });
            });

            await app.RunAsync(args);
        }
    }
}
