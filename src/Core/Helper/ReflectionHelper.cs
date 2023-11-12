using Core.Abstractions;
using Core.Attributes;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Core.Helper
{
    /// <summary>
    /// Represents a helper class when dealing with reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Creates instances of the classes implementing the interface given in <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The interface type.</typeparam>
        /// <returns>Instances of all classes implementing <typeparamref name="TInterface"/>.</returns>
        public static IEnumerable<TInterface> InitializeClassesImplementingInterface<TInterface>()
        {
            var output = new List<TInterface>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var classes = GetClassesImplementingInterface<TInterface>(assembly);
                foreach (var classType in classes)
                {
                    var instance = CreateInstance<TInterface>(classType);
                    if (instance is not null)
                    {
                        output.Add(instance);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Gets all classes implementing the interface given in <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Collection of concrete types.</returns>
        public static IEnumerable<Type> GetClassesImplementingInterface<TInterface>(Assembly assembly)
        {
            var interfaceType = typeof(TInterface);
            var classes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
                .ToList();

            return classes;
        }

        /// <summary>
        /// Gets a single class implementing the interface given in <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="className">Name of the class.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <returns>The type of the class.</returns>
        /// <exception cref="SourceNotFoundException"></exception>
        public static Type GetClassByNameImplementingInterface<TInterface>(string className, IServiceProvider serviceProvider)
        {
            LoadExternalAssemblies(serviceProvider);

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in allAssemblies)
            {
                var foundClass = GetClassesImplementingInterface<TInterface>(assembly).FirstOrDefault(x => x.Name == className);
                if (foundClass is not null)
                {
                    return foundClass;
                }
            }

            var localizer = serviceProvider.GetServiceDirectly<IStringLocalizer<CoreLayerLocalization>>(true);
            throw new SourceNotFoundException(className, localizer!);
        }

        /// <summary>
        /// Creates an instance of type <paramref name="classType"/> casted to <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The resulting interface type.</typeparam>
        /// <param name="classType">Type of the class.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <returns>The concrete type of <paramref name="classType"/> casted to <typeparamref name="TInterface"/>.</returns>
        public static TInterface? CreateInstance<TInterface>(Type classType, IServiceProvider? serviceProvider = null)
        {
            if (serviceProvider is null)
            {
                return (TInterface?)Activator.CreateInstance(classType);
            }

            var arguments = new List<object?>();
            var constructorsWithDescendingParamsCount = classType
                .GetConstructors()
                .OrderByDescending(x => x.GetParameters().Length);

            foreach (var ctor in constructorsWithDescendingParamsCount)
            {
                var parameters = ctor.GetParameters();
                var parameterInstances = parameters.Select(
                    x => serviceProvider.GetServiceDirectly(x.ParameterType));

                arguments.Clear();
                arguments.AddRange(parameterInstances);

                if (parameters.Length == 0)
                {
                    return (TInterface?)Activator.CreateInstance(classType);
                }
                else if (arguments.Count == parameters.Length)
                {
                    return (TInterface?)Activator.CreateInstance(classType, arguments.ToArray());
                }
            }

            return (TInterface?)Activator.CreateInstance(classType);
        }

        /// <summary>
        /// Gets key-value pairs of all public instance properties decorated with <see cref="DetailViewColumnAttribute"/> of <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Zero or more key-value pairs.</returns>
        public static IEnumerable<KeyValuePair<string, object?>> GetProperties(object obj)
        {
            if (obj is null)
            {
                return Enumerable.Empty<KeyValuePair<string, object?>>();
            }

            var output = new List<KeyValuePair<string, object>>();

            PropertyInfo[] properties = obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(DetailViewColumnAttribute)) is DetailViewColumnAttribute resultAttribute)
                {
                    object? propertyValue = property.GetValue(obj);
                    if (propertyValue is null)
                    {
                        output.Add(new(property.Name, null!));
                    }
                    else
                    {
                        object castedValue = Convert.ChangeType(propertyValue, resultAttribute.TargetType);
                        output.Add(new(property.Name, castedValue));
                    }
                }
            }

            return output!;
        }

        private static void LoadExternalAssemblies(IServiceProvider serviceProvider)
        {
            var pluginService = serviceProvider.GetServiceDirectly<IPluginService>(true)!;

            if (!pluginService.IsAllowed())
            {
                return;
            }

            pluginService.ActivateExternalPluginsIfAllowed();
        }
    }
}
