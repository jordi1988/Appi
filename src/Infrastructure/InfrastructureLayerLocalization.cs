using Microsoft.Extensions.Localization;

[assembly: RootNamespace("Infrastructure")]

namespace Infrastructure
{
    /// <summary>
    /// Marker class for localization purposes.
    /// </summary>
    /// <remarks>The namespace of this abstract class lacks the 'Abstraction' path, because that way the filename of the .resx file is much more readable.</remarks>
    public abstract class InfrastructureLayerLocalization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfrastructureLayerLocalization"/> class.
        /// </summary>
        protected InfrastructureLayerLocalization()
        {
        }
    }
}
