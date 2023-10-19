using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Ui.Appi.Injection
{
    /// <inheritdoc cref="ITypeResolver" />
    internal sealed class TypeResolver : ITypeResolver
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolver"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <exception cref="ArgumentNullException">provider</exception>
        public TypeResolver(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc cref="ITypeResolver.Resolve(Type?)" />
        public object? Resolve(Type? type)
        {
            return _provider.GetRequiredService(type!);
        }
    }
}
