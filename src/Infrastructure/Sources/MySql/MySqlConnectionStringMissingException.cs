using Core.Exceptions;

namespace Infrastructure.MySql
{
    /// <summary>
    /// Represents the <see cref="MySqlConnectionStringMissingException"/> class which will be used if the connection string was not provided.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class MySqlConnectionStringMissingException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlConnectionStringMissingException"/> class.
        /// </summary>
        public MySqlConnectionStringMissingException()
            : base($"MySQL connection string must be provided through `Arguments` property in configuration file.")
        {
        }
    }
}
