using Spectre.Console.Cli;
using Ui.Appi.Commands;

namespace Ui.Appi
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var app = new CommandApp<FindItemsCommand>();
            app.Configure(config =>
            {
                config.AddCommand<FindItemsCommand>("find");
                config.AddBranch("config", source =>
                {
                    source.AddCommand<ConfigOpenDirectoryCommand>("open").WithDescription("Opens the app's configuration directory.");
                    source.AddCommand<ConfigAllowLibrariesCommand>("allow-libs").WithDescription("Allow or disallow external libraries to be loaded.");
                });
            });

            await app.RunAsync(args);
        }
    }
}
