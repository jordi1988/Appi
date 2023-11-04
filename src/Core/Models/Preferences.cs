using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Core.Models
{
    /// <summary>
    /// Represents the app's preferences.
    /// </summary>
    public class Preferences
    {
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public string UiCulture { get; set; } = CultureInfo.CurrentUICulture.Name;

        /// <summary>
        /// Gets or sets the source colors for the legend.
        /// </summary>
        public LegendPreferences Legend { get; set; } = new();
    }
}
