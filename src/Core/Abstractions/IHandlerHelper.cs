using Core.Models;

namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface which can be used by the client.
    /// </summary>
    public interface IHandlerHelper
    {
        /// <summary>
        /// Provides backward navigation.
        /// </summary>
        ActionItem Back();

        /// <summary>
        /// Privides exit navigation.
        /// </summary>
        ActionItem Exit();

        /// <summary>
        /// Escapes the markup in the output.
        /// </summary>
        /// <param name="text">The text where the markup should be escaped.</param>
        /// <returns>The text with escaped markup.</returns>
        string EscapeMarkup(string? text);

        /// <summary>
        /// Removes the markup in the output.
        /// </summary>
        /// <param name="text">The text where the markup should be removed.</param>
        /// <returns>The text with removed markup.</returns>
        string RemoveMarkup(string? text);
    }
}
