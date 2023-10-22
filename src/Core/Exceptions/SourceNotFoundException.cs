using Microsoft.Extensions.Localization;

namespace Core.Exceptions
{
    /// <summary>
    /// Represents the <see cref="SourceNotFoundException"/> class which will be used if a given source name is not found.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class SourceNotFoundException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceNotFoundException"/> class.
        /// </summary>
        /// <param name="sourceName">The missing source name.</param>
        /// <param name="localizer">The localizer.</param>
        public SourceNotFoundException(string sourceName, IStringLocalizer<CoreLayerLocalization> localizer)
            : base(localizer["The source '{0}' could not be found.", sourceName])
        {
        }
    }
}
