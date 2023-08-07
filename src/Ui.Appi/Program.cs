using Core.Services;
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
            app.Configure(config => ConfigureCommands(config));
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

            return new TypeRegistrar(services);
        }

        private static void ConfigureCommands(IConfigurator config)
        {
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("Appi");
            config.ValidateExamples();

            config.AddCommand<FindItemsCommand>("find");
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
    }
}
