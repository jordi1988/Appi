using Core.Exceptions;

namespace Infrastructure.MsSql
{
    public class MsSqlConnectionStringMissingException : CoreException
    {
        public MsSqlConnectionStringMissingException()
            : base($"Microsoft SQL connection string must be provided through `Arguments` property in configuration file.")
        {
        }
    }
}
