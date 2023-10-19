using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Ui.Appi.Injection
{
    /// <inheritdoc cref="ITypeRegistrar"/>
    public sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistrar" /> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public TypeRegistrar(IServiceCollection builder)
        {
            _builder = builder;
        }

        /// <inheritdoc cref="ITypeRegistrar.Build" />
        public ITypeResolver Build()
        {
            return new TypeResolver(_builder.BuildServiceProvider());
        }

        /// <inheritdoc cref="ITypeRegistrar.Register(Type, Type)" />
        public void Register(Type service, Type implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterInstance(Type, object)" />
        public void RegisterInstance(Type service, object implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterLazy(Type, Func{object})" />
        public void RegisterLazy(Type service, Func<object> factory)
        {
            _builder.AddSingleton(service, factory);
        }
    }
}
