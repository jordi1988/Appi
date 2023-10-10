using Core.Abstractions;
using Core.Strategies;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Ui.Appi.Commands;
using Ui.Appi.Injection;

namespace Ui.Appi
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var app = new CommandApp(RegisterServices());
            app.Configure(ConfigureCommands);
            app.SetDefaultCommand<FindItemsCommand>();

            await app.RunAsync(args);
        }

        private static ITypeRegistrar RegisterServices()
        {
            var services = new ServiceCollection();

            // Core components
            services.AddSingleton<IHandler, SpectreConsoleHandler>();
            services.AddSingleton<IHandlerHelper, SpectreConsoleHandlerHelper>();
            services.AddScoped<ISettingsService, FileSettingsService>();
            services.AddScoped<QueryStrategyCalculator>();

            // Infrastructure components
            services.AddPluginService();

            // Ui components
            services.AddScoped<EmptyCommandSettings>();
            services.AddScoped<FindItemsCommand.Settings>();
            services.AddScoped<ConfigAllowLibrariesCommand.Settings>();
            services.AddScoped<ConfigRegisterLibraryCommand.Settings>();

            return new TypeRegistrar(services);
        }

        private static void ConfigureCommands(IConfigurator config)
        {
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("Appi");
            config.ValidateExamples();

            config.AddCommand<FindItemsCommand>("find")
                .WithDescription("Query all [i](default)[/] or only the specified sources.")
                .WithExample("god")
                .WithExample("god", "-s", "poetry")
                .WithExample("god", "-g", "demo")
                .WithExample("find", "god")
                .WithExample("find", "god", "--source", "poetry")
                .WithExample("find", "god", "--group", "demo");

            config.AddCommand<ListSourcesCommand>("list")
                .WithDescription("List all sources usable for the [i]find[/] command.")
                .WithExample("list");

            config.AddBranch("config", source =>
            {
                source.SetDescription("Configure Appi.");

                source.AddCommand<ConfigOpenDirectoryCommand>("open")
                    .WithDescription("Opens the app's configuration directory.")
                    .WithExample("config", "open");

                source.AddCommand<ConfigAllowLibrariesCommand>("allow-libs")
                    .WithDescription("Allow or disallow external libraries to be loaded.")
                    .WithExample("config", "allow-libs", "true");

                source.AddCommand<ConfigRegisterLibraryCommand>("register-lib")
                    .WithDescription("Register a new library which is copied to application directory and registered in `sources.json`.")
                    .WithExample("config", "register-lib", @"E:\my-own-appi-plugin.dll");
            });
        }
    }
}
