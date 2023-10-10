using Core.Abstractions;
using Core.Attributes;
using Core.Exceptions;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Core.Helper
{
    public static class ReflectionHelper
    {
        public static List<T> InitializeClassesImplementingInterface<T>(object? settings = null)
        {
            var output = new List<T>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var classes = GetClassesImplementingInterface<T>(assembly);
                foreach (var classType in classes)
                {
                    T instance = CreateInstance<T>(classType, settings);
                    output.Add(instance);
                }
            }

            return output;
        }

        public static List<Type> GetClassesImplementingInterface<T>(Assembly assembly)
        {
            var interfaceType = typeof(T);
            var classes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
                .ToList();

            return classes;
        }

        public static Type GetClassByNameImplementingInterface<T>(string className, IPluginService pluginService)
        {
            LoadExternalAssemblies(pluginService);

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in allAssemblies)
            {
                var foundClass = GetClassesImplementingInterface<T>(assembly).Find(x => x.Name == className);
                if (foundClass is not null)
                {
                    return foundClass;
                }
            }

            throw new SourceNotFoundException(className);
        }

        public static T CreateInstance<T>(Type classType, object? firstParameter = null)
        {
            if (firstParameter is null)
            {
                return (T)Activator.CreateInstance(classType);
            }

            var constructorWithOneParameterOfGivenType = classType.GetConstructor(new[] { firstParameter.GetType() });
            if (constructorWithOneParameterOfGivenType is not null)
            {
                return (T)Activator.CreateInstance(classType, firstParameter);
            }

            return (T)Activator.CreateInstance(classType);
        }

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
