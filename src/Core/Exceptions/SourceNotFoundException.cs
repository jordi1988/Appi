namespace Core.Exceptions
{
    /// <summary>
    /// Represents the <see cref="SourceNotFoundException"/> class which will be used if a given source name is not found.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class SourceNotFoundException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceNotFoundException"/> class.
        /// </summary>
        /// <param name="sourceName">The missing source name.</param>
        public SourceNotFoundException(string sourceName)
            : base($"The source `{sourceName}` could not be found.")
        {
        }
    }
}
