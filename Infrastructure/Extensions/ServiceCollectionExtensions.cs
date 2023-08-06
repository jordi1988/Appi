using Core.Abstractions;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPluginService(this IServiceCollection services)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException($"Sorry, `${typeof(IPluginService).Name}` can only be used with Windows OS for now.");
            }

            services.AddSingleton<IPluginService, WindowsPluginService>();

            return services;
        }
    }
}
