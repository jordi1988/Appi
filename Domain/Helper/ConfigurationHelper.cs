using Core.Abstractions;
using System.Text.Json;
using static System.Environment;

namespace Core.Helper
{
    public static class ConfigurationHelper
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
    }
}
