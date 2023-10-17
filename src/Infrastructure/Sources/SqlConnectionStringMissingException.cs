using Core.Exceptions;

namespace Infrastructure.Sources
{
    /// <summary>
    /// Represents the <see cref="SqlConnectionStringMissingException"/> class which will be used if the connection string was not provided.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class SqlConnectionStringMissingException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConnectionStringMissingException"/> class.
        /// </summary>
        public SqlConnectionStringMissingException()
            : base($"SQL connection string must be provided through `Arguments` property in configuration file.")
        {
        }
    }
}
