using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    /// <summary>
    /// Represents an extension class to the service provider.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets the desired service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="throwIfNull">if set to <c>true</c> throw if null.</param>
        /// <returns>Implementation instance.</returns>
        /// <exception cref="System.NotImplementedException">Thrown if <typeparamref name="T"/> has no implementation.</exception>
        public static T? GetServiceDirectly<T>(this IServiceProvider serviceProvider, bool throwIfNull = false)
        {
            var service = GetServiceDirectly(serviceProvider, typeof(T), throwIfNull);
            if (service is null)
            {
                return default;
            }

            return (T)service;
        }

        /// <summary>
        /// Gets the desired service.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="type"></param>
        /// <param name="throwIfNull">if set to <c>true</c> throw if null.</param>
        /// <returns>Implementation instance.</returns>
        /// <exception cref="System.NotImplementedException">Thrown if <typeparamref name="T"/> has no implementation.</exception>
        public static object? GetServiceDirectly(this IServiceProvider serviceProvider, Type type, bool throwIfNull = false)
        {
            using IServiceScope serviceScope = serviceProvider.CreateScope();

            IServiceProvider provider = serviceScope.ServiceProvider;
            var service = provider.GetService(type);

            if (throwIfNull && service is null)
            {
                throw new NotImplementedException(type?.GetType().Name);
            }

            return service;
        }
    }
}
