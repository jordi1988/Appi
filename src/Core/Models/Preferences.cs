using System.Globalization;

namespace Core.Models
{
    /// <summary>
    /// Represents the app's preferences.
    /// </summary>
    public class Preferences
    {
        /// <summary>
        /// Gets or sets the application directory of <c>Appi</c>.
        /// </summary>
        /// <remarks>Can be viewed with `appi config open` command.</remarks>
        public string AppDataDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full filename for the `sources.json`.
        /// </summary>
        public string SourcesFilename { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        public string UiCulture { get; set; } = CultureInfo.CurrentUICulture.Name;

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <remarks>Item count per page.</remarks>
        public int PageSize { get; set; } = 35;

        /// <summary>
        /// Gets or sets the color of the accent.
        /// </summary>
        /// <remarks>See <see href="https://spectreconsole.net/appendix/colors">all available colors</see>.</remarks>
        public string AccentColor { get; set; } = "Red";

        /// <summary>
        /// Gets or sets the source colors for the legend.
        /// </summary>
        public LegendPreferences Legend { get; set; } = new();
    }
}
