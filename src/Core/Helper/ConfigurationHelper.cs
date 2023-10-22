using Core.Abstractions;
using System.Text.Json;
using static System.Environment;

namespace Core.Helper
{
    /// <summary>
    /// Represents a helper class when dealing with configuration.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// The query parameter if needed as variable.
        /// </summary>
        /// <remarks>See <see href="https://github.com/jordi1988/Appi/blob/master/examples/HttpRequestDemo/PoetryHttpRequestSource.cs">example usage</see>.</remarks>
        public const string QueryParam = "##QUERY##";

        /// <summary>
        /// Gets the application directory of <c>Appi</c>.
        /// </summary>
        /// <value>
        /// The application directory.
        /// </value>
        /// <remarks>Can be viewed with `appi config open` command.</remarks>
        public static string ApplicationDirectory => Path.Combine(_appDataDirectory, "Appi");

        /// <summary>
        /// Gets the application setting's filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public static string ApplicationFilename => Path.Combine(ApplicationDirectory, "sources.json");

        private static string _appDataDirectory => GetFolderPath(SpecialFolder.ApplicationData);

        static ConfigurationHelper()
        {
            EnsureSettingsExist();
        }

        /// <summary>
        /// Ensures the settings file exists.
        /// </summary>
        /// <remarks>Will recreate the file with defaults if not found.</remarks>
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
                var sources = ReflectionHelper.InitializeClassesImplementingInterface<ISource>()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                    .OrderBy(x => x.SortOrder);

                var stringifiedSources = JsonSerializer.Serialize(sources,
                    new JsonSerializerOptions() { WriteIndented = true });

                File.WriteAllText(ApplicationFilename, stringifiedSources);
            }
        }
    }
}
