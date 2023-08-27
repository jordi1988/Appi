using Core.Services;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Ui.Appi.Commands;
using Ui.Appi.Injection;

namespace Ui.Appi
{
    internal static class Program
    {
        private static ServiceProvider? _serviceProvider;

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
            services.AddScoped<SourceService>();

            // Infrastructure
            services.AddPluginService();

            // Ui components
            services.AddScoped<EmptyCommandSettings>();
            services.AddScoped<FindItemsCommand.Settings>();
            services.AddScoped<ConfigAllowLibrariesCommand.Settings>();
            services.AddScoped<ConfigRegisterLibraryCommand.Settings>();

            _serviceProvider = services.BuildServiceProvider();

            return new TypeRegistrar(services);
        }

        private static void ConfigureCommands(IConfigurator config)
        {
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("Appi");
            config.ValidateExamples();

            config.AddCommand<FindItemsCommand>("find")
                .WithDescription("Query all [i](default)[/] or only the specified sources")
                .WithExample("god")
                .WithExample("find", "god")
                .WithExample("find", "god", "--source", "poetry")
                .WithExample("find", "god", "--group", "demo");

            // config.AddCustomFindItemsCommandsFromAliases();

            // TODO: command for listing groups and sources

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
                    .WithDescription("Register a new library which is copied to application directory and registred in sources.json.")
                    .WithExample("config", "register-lib", @"E:\appi-plugin2.dll");
            });
        }

        /*
        private static void AddCustomFindItemsCommandsFromAliases(this IConfigurator config)
        {
            var sourceService = _serviceProvider?.GetService(typeof(SourceService)) as SourceService;
            var sources = sourceService!
                .ReadSettingsFileSources()
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.Alias) &&
                    x.IsQueryCommand == true);

            foreach (var source in sources)
            {
                config.AddCommand<FindItemsCommand>(source.Alias)
                    .WithData(new FindItemsCommandData(source.Alias))
                    .WithDescription($"Query the source `[i]{source.Name}[/]` directly")
                    .WithExample(source.Alias, "god");
            }
        }
        */
    }
}
