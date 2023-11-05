using Core.Abstractions;
using Core.Models;
using System.Drawing;
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
        public static string ApplicationDirectory => Path.Combine(AppDataDirectory, "Appi");

        /// <summary>
        /// Gets the application setting's filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public static string SourcesFilename => Path.Combine(ApplicationDirectory, "sources.json");

        /// <summary>
        /// Gets the application preferences' filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public static string PreferencesFilename => Path.Combine(ApplicationDirectory, "preferences.json");

        private static string AppDataDirectory => GetFolderPath(SpecialFolder.ApplicationData);
                
        /// <summary>
        /// Ensures all settings used in the application exists.
        /// </summary>
        /// <remarks>Will recreate the file with defaults if not found.</remarks>
        public static void EnsureSettingsExist()
        {
            EnsureDirectoryExists();
            EnsureFileExists(
                SourcesFilename,
                () => ReflectionHelper.InitializeClassesImplementingInterface<ISource>()
                        .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                        .OrderBy(x => x.SortOrder));

            EnsureFileExists(
                    PreferencesFilename,
                    () => new Preferences()
                    {
                        Legend = new LegendPreferences()
                        {
                            SourceColors = GetLegendSourceDefaultColors()
                        }
                    });
        }

        private static void EnsureDirectoryExists()
        {
            if (Directory.Exists(ApplicationDirectory))
            {
                return;
            }

            Directory.CreateDirectory(ApplicationDirectory);
        }

        private static void EnsureFileExists(string filename, Func<object> defaultValue)
        {
            if (File.Exists(filename))
            {
                return;
            }

            var stringifiedObject = JsonSerializer.Serialize(defaultValue(),
                new JsonSerializerOptions() { WriteIndented = true });

            File.WriteAllText(filename, stringifiedObject);
        }

        private static string[] GetLegendSourceDefaultColors() => new List<Color>() {
            Color.SkyBlue,
            Color.Magenta,
            Color.IndianRed,
            Color.LightGoldenrodYellow,
            Color.LightGreen,
            Color.Blue,
            Color.LightPink,
            Color.LightSeaGreen,
            Color.NavajoWhite,
            Color.Olive,
            Color.DarkRed,
            Color.GreenYellow,
            Color.PaleGreen,
            Color.SandyBrown,
            Color.SlateGray,
            Color.Turquoise,
            Color.Wheat,
            Color.Coral,
            Color.ForestGreen,
            Color.Orange
        }.Select(color => color.ToKnownColor().ToString())
         .ToArray();
    }
}
