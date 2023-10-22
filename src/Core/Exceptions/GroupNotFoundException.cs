using Microsoft.Extensions.Localization;

namespace Core.Exceptions
{
    /// <summary>
    /// Represents the <see cref="GroupNotFoundException"/> class which will be used if a given group alias name is not found.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class GroupNotFoundException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupNotFoundException"/> class.
        /// </summary>
        /// <param name="groupAlias">The missing group alias name.</param>
        /// <param name="localizer">The localizer.</param>
        public GroupNotFoundException(string groupAlias, IStringLocalizer<CoreLayerLocalization> localizer)
            : base(localizer["The provided group '{0}' does not contain any sources.", groupAlias])
        {
        }
    }
}
