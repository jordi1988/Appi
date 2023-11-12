namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface for interacting with custom plugins.
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        /// Determines whether plugins are allowed.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if plugins are allowed; otherwise, <c>false</c>.
        /// </returns>
        bool IsAllowed();

        /// <summary>
        /// Allow external plugins to be used.
        /// </summary>
        void Allow();

        /// <summary>
        /// Prohibit external plugins to be used.
        /// </summary>
        void Prohibit();

        /// <summary>
        /// Loads all assemblies in the known directories if <see cref="IsAllowed"/> returns <c>true</c>.
        /// </summary>
        void ActivateExternalPluginsIfAllowed();
    }
}
