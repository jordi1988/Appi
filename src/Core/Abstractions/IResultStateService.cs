using Core.Models;

namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface for managing the result state.
    /// </summary>
    public interface IResultStateService
    {
        /// <summary>
        /// Saves the results internally to load them later.
        /// </summary>
        /// <param name="results">The results.</param>
        void Save(IEnumerable<PromptGroup> results);

        /// <summary>
        /// Loads the previously saved results.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PromptGroup> Load();
    }
}
