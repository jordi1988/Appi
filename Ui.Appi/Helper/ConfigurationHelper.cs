using Domain.Interfaces;
using System.Text.Json;
using static System.Environment;
using Settings = Ui.Appi.Commands.FindItemsCommand.Settings;

namespace Ui.Appi.Helper
{
    internal static partial class ConfigurationHelper
    {
        public const string QueryParam = "##QUERY##";
        public static string ApplicationDirectory => Path.Combine(_appDataDirectory, "Appi");
        public static string ApplicationFilename => Path.Combine(ApplicationDirectory, "sources.json");
        private static string _appDataDirectory => GetFolderPath(SpecialFolder.ApplicationData);

        static ConfigurationHelper()
        {
            EnsureSettingsExist();
        }

        public static void EnsureSettingsExist()
        {
            EnsureDirectoryExists();
            EnsureFileExists();
        }

        public static IEnumerable<ISource> GetActiveSources(Settings? settings, IExternalLibraryService externalLibraryService)
        {
            var output = new List<ISource>();

            var settingsFileActiveSources = ReadSettingsFileSources().Where(x => x.IsActive);
            foreach (var source in settingsFileActiveSources)
            {
                var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, externalLibraryService);
                var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass, settings);

                source.CopyTo(instance);

                output.Add(instance);
            }

            return output;
        }

        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(ApplicationDirectory))
            {
                Directory.CreateDirectory(ApplicationDirectory);
            }
        }

        private static void EnsureFileExists()
        {
            if (!File.Exists(ApplicationFilename))
            {
                var sources = ReflectionHelper.InitializeClassesImplementingInterface<ISource>(new())
                    .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                    .OrderBy(x => x.SortOrder);

                var stringifiedSources = JsonSerializer.Serialize(sources,
                    new JsonSerializerOptions() { WriteIndented = true });

                File.WriteAllText(ApplicationFilename, stringifiedSources);
            }
        }

        private static IEnumerable<ISource> ReadSettingsFileSources()
        {
            EnsureSettingsExist();

            var fileContents = File.ReadAllText(ApplicationFilename);
            var fileSources = JsonSerializer.Deserialize<IEnumerable<DeserializationSource>>(fileContents);

            return fileSources ?? Enumerable.Empty<ISource>();
        }

        private static void CopyTo(this ISource source, ISource instance)
        {
            instance.Name = source.Name;
            instance.Description = source.Description;
            instance.IsActive = true;
            instance.SortOrder = source.SortOrder;
            instance.Path = source.Path;
        }
    }
}
