using Core.Abstractions;
using Core.Extensions;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// Extends the <see cref="ServiceCollection"/> type.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the plugin service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="InvalidOperationException">Only Windows OS is currently supported.</exception>
        public static IServiceCollection AddPluginService(this IServiceCollection services)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException($"Sorry, '{typeof(IPluginService).Name}' can only be used with Windows OS for now.");
            }

            services.AddSingleton<IPluginService, WindowsPluginService>();

            return services;
        }

        /// <summary>
        /// Adds services from custom plugins.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="provider">The provider</param>
        /// <remarks>This method is the reason for custom services to be nullable when creating the instances before the custom service gets registered.</remarks>
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IServiceProvider provider)
        {
            var settingsService = provider.GetServiceDirectly<ISettingsService>(true);
            var sources = settingsService!.ReadSources();

            var instances = sources.ToRealInstance(provider);
            foreach (var instance in instances)
            {
                instance.AddCustomServices(services);
            }

            return services;
        }
    }
}
