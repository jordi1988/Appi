namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface for determing the query strategy.
    /// </summary>
    public interface ISourceStrategy
    {
        /// <summary>
        /// Gets the query description strategy.
        /// </summary>
        /// <value>
        /// The query description strategy.
        /// </value>
        string QueryWithinDescription { get; }

        /// <summary>
        /// Gets the sources using the specific strategy.
        /// </summary>
        /// <returns>All sources matching the strategy.</returns>
        IEnumerable<ISource> GetSources();
    }
}
