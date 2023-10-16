using Core.Abstractions;
using Core.Attributes;
using Core.Exceptions;
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
        /// <param name="firstConstructorParameter">The first parameter of the constructor of any type.</param>
        /// <returns>Instances of all classes implementing <typeparamref name="TInterface"/>.</returns>
        public static IEnumerable<TInterface> InitializeClassesImplementingInterface<TInterface>(object? firstConstructorParameter = null)
        {
            var output = new List<TInterface>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var classes = GetClassesImplementingInterface<TInterface>(assembly);
                foreach (var classType in classes)
                {
                    TInterface instance = CreateInstance<TInterface>(classType, firstConstructorParameter);
                    output.Add(instance);
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
        /// <param name="pluginService">The plugin service.</param>
        /// <returns>The type of the class.</returns>
        /// <exception cref="SourceNotFoundException"></exception>
        public static Type GetClassByNameImplementingInterface<TInterface>(string className, IPluginService pluginService)
        {
            LoadExternalAssemblies(pluginService);

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in allAssemblies)
            {
                var foundClass = GetClassesImplementingInterface<TInterface>(assembly).FirstOrDefault(x => x.Name == className);
                if (foundClass is not null)
                {
                    return foundClass;
                }
            }

            throw new SourceNotFoundException(className);
        }

        /// <summary>
        /// Creates an instance of type <paramref name="classType"/> constructed with <paramref name="firstParameter"/> and casted to <typeparamref name="TInterface"/>.
        /// </summary>
        /// <typeparam name="TInterface">The resulting interface type.</typeparam>
        /// <param name="classType">Type of the class.</param>
        /// <param name="firstParameter">The first constructor parameter.</param>
        /// <returns>The concrete type of <paramref name="classType"/> casted to <typeparamref name="TInterface"/>.</returns>
        public static TInterface CreateInstance<TInterface>(Type classType, object? firstParameter = null)
        {
            if (firstParameter is null)
            {
                return (TInterface)Activator.CreateInstance(classType);
            }

            var constructorWithOneParameterOfGivenType = classType.GetConstructor(new[] { firstParameter.GetType() });
            if (constructorWithOneParameterOfGivenType is not null)
            {
                return (TInterface)Activator.CreateInstance(classType, firstParameter);
            }

            return (TInterface)Activator.CreateInstance(classType);
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

        private static void LoadExternalAssemblies(IPluginService pluginService)
        {
            ConfigurationHelper.EnsureSettingsExist();
            if (!pluginService.IsAllowed())
            {
                return;
            }

            UnsafeLoadAssemblies(ConfigurationHelper.ApplicationDirectory, "*.dll");

            var executablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            UnsafeLoadAssemblies(executablePath, "Appi.Plugin.*.dll");
        }

        private static void UnsafeLoadAssemblies(string path, string searchPattern)
        {
            var externalAssemblyFiles = Directory.GetFiles(path, searchPattern);
            foreach (var filename in externalAssemblyFiles)
            {
                Assembly.UnsafeLoadFrom(filename);
            }
        }
    }
}
