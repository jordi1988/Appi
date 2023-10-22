using Microsoft.Extensions.Localization;

[assembly: RootNamespace("Core")]

namespace Core
{
    /// <summary>
    /// Marker class for localization purposes.
    /// </summary>
    /// <remarks>The namespace of this abstract class lacks the 'Abstraction' path, because that way the filename of the .resx file is much more readable.</remarks>
    public abstract class CoreLayerLocalization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreLayerLocalization"/> class.
        /// </summary>
        protected CoreLayerLocalization()
        {
        }
    }
}
