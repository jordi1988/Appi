namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface for interacting with the app's settings.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Reads the sources.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ISource> ReadSources();

        /// <summary>
        /// Write the sources.
        /// </summary>
        /// <param name="sources">The sources.</param>
        void SaveSources(IEnumerable<ISource> sources);
    }
}
