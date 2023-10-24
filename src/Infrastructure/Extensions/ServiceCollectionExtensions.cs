using Core.Abstractions;
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
    }
}
