using Core.Models;

namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface for handling the output.
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Prints all registered sources that can be used with <c>find</c> command.
        /// </summary>
        /// <param name="sources">The sources.</param>
        void ListSources(IEnumerable<ISource> sources);

        /// <summary>
        /// Prints the results that were found by the <c>find</c> command.
        /// </summary>
        /// <param name="results">The results.</param>
        void PrintResults(IEnumerable<PromptGroup> results);
        
        /// <summary>
        /// Clears the screen.
        /// </summary>
        void ClearScreen();
    }
}
