using Core.Exceptions;

namespace Infrastructure.MySql
{
    public class MySqlConnectionStringMissingException : CoreException
    {
        public MySqlConnectionStringMissingException()
            : base($"MySQL connection string must be provided through `Arguments` property in configuration file.")
        {
        }
    }
}
