using Core.Abstractions;

namespace Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="IResultStateService{T}"/> using a non-persistent memory approach.
    /// </summary>
    /// <seealso cref="IResultStateService{T}" />
    public class MemoryResultStateService<T> : IResultStateService<T>
        where T : class
    {
        private IEnumerable<T>? _results;

        /// <summary>
        /// Loads the previously saved results from memory.
        /// </summary>
        /// <returns>Previously saved state.</returns>
        public IEnumerable<T> Load()
        {
            return _results ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Saves the results internally into the memory to load them later.
        /// </summary>
        /// <param name="results">The state.</param>
        public void Save(IEnumerable<T> results)
        {
            _results = results;
        }
    }
}
