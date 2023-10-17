namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface for managing the result state.
    /// </summary>
    public interface IResultStateService<T>
        where T : class
    {
        /// <summary>
        /// Saves the results internally to load them later.
        /// </summary>
        /// <param name="results">The results.</param>
        void Save(IEnumerable<T> results);

        /// <summary>
        /// Loads the previously saved results.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Load();
    }
}
