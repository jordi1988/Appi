using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Ui.Appi.Injection
{
    /// <inheritdoc cref="ITypeRegistrar"/>
    public sealed class TypeRegistrar : ITypeRegistrar
    {
        /// <summary>
        /// Gets the service collection.
        /// </summary>
        /// <value>
        /// The builder.
        /// </value>
        public IServiceCollection Builder { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistrar" /> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public TypeRegistrar(IServiceCollection builder)
        {
            Builder = builder;
        }

        /// <inheritdoc cref="ITypeRegistrar.Build" />
        public ITypeResolver Build()
        {
            return new TypeResolver(Builder.BuildServiceProvider());
        }

        /// <inheritdoc cref="ITypeRegistrar.Register(Type, Type)" />
        public void Register(Type service, Type implementation)
        {
            Builder.AddSingleton(service, implementation);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterInstance(Type, object)" />
        public void RegisterInstance(Type service, object implementation)
        {
            Builder.AddSingleton(service, implementation);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterLazy(Type, Func{object})" />
        public void RegisterLazy(Type service, Func<object> factory)
        {
            Builder.AddSingleton(service, factory);
        }
    }
}
