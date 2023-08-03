using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Reflection;
using static Ui.Appi.Commands.FindItemsCommand;

namespace Ui.Appi.Helper
{
    internal static class ReflectionHelper
    {
        public static List<T> InitializeClassesImplementingInterface<T>(Settings? settings = null)
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

        public static Type GetClassByNameImplementingInterface<T>(string className, IExternalLibraryService externalLibraryService)
        {
            LoadExternalAssemblies(externalLibraryService);

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

        private static void LoadExternalAssemblies(IExternalLibraryService externalLibraryService)
        {
            ConfigurationHelper.EnsureSettingsExist();
            if (!externalLibraryService.IsAllowed())
            {
                return;
            }

            var externalAssemblyFiles = Directory.GetFiles(ConfigurationHelper.ApplicationDirectory, "*.dll");
            foreach (var filename in externalAssemblyFiles)
            {
                Assembly.UnsafeLoadFrom(filename);
            }
        }

        public static T CreateInstance<T>(Type classType, Settings? settings = null)
        {
            if (settings is null)
            {
                return (T)Activator.CreateInstance(classType);
            }

            var constructorContainsSettingsParameter = classType.GetConstructor(new[] { typeof(Settings) });
            if (constructorContainsSettingsParameter is not null)
            {
                return (T)Activator.CreateInstance(classType, settings!);
            }
            else
            {
                return (T)Activator.CreateInstance(classType);
            }
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
                if (property.GetCustomAttribute(typeof(ResultAttribute)) is ResultAttribute resultAttribute)
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
    }
}
