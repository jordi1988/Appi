namespace Core.Exceptions
{
    /// <summary>
    /// Base exception class of the core layer.
    /// </summary>
    /// <seealso cref="Exception" />
    public class CoreException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CoreException(string? message) : base(message)
        {
        }
    }
}
