using System.Drawing;

namespace Core.Models
{
    /// <summary>
    /// Represents preferences related to the legend.
    /// </summary>
    public class LegendPreferences
    {
        /// <summary>
        /// Gets or sets a value indicating whether the legend is printed or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the source colors for the legend.
        /// </summary>
        /// <value>
        /// The source colors.
        /// </value>
        /// <remarks>Choose any color from <see cref="Color"/>.</remarks>
        public List<string> SourceColors { get; set; } = new List<string>() {
              "DarkRed",
              "SkyBlue",
              "Magenta",
              "Olive",
              "IndianRed",
            "NavajoWhite",
              "LightGoldenrodYellow",
              "LightGreen",
              "Blue",
              "LightPink",
              "LightSeaGreen",
              "GreenYellow",
              "Orange",
              "PaleGreen",
              "SandyBrown"
        };
    }
}
