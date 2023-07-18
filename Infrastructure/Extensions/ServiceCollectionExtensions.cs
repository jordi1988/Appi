using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExternalLibraryService(this IServiceCollection services)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException("Sorry, `ExternalLibraryService` can only be used with Windows OS for now.");
            }

            services.AddSingleton<IExternalLibraryService, WindowsExternalLibraryService>();

            return services;
        }
    }
}
