using Core.Abstractions;
using Core.Helper;
using Core.Models;
using System.Drawing;
using System.Text.Json;
using static System.Environment;

namespace Ui.Appi.Helper
{
    /// <summary>
    /// Represents a helper class when dealing with configuration.
    /// </summary>
    public static class ConfigurationHelper
    {
        private static string OSAppDataDirectory => GetFolderPath(SpecialFolder.ApplicationData);
        private static string AppDataDirectory => Path.Combine(OSAppDataDirectory, "Appi");
        private static string SourcesFilename => Path.Combine(AppDataDirectory, "sources.json");

        /// <summary>
        /// Gets the application preferences' filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public static string PreferencesFilename => Path.Combine(AppDataDirectory, "preferences.json");
        
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
                        AppDataDirectory = AppDataDirectory,
                        SourcesFilename = SourcesFilename,
                        Legend = new LegendPreferences()
                        {
                            SourceColors = GetLegendSourceDefaultColors()
                        }
                    });
        }

        private static void EnsureDirectoryExists()
        {
            if (Directory.Exists(AppDataDirectory))
            {
                return;
            }

            Directory.CreateDirectory(AppDataDirectory);
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
