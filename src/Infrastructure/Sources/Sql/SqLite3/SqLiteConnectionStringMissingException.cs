using Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Sources.Sql.SqLite3
{
    /// <summary>
    /// Represents the <see cref="SqLiteConnectionStringMissingException"/> class which will be used if the connection string was not provided.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class SqLiteConnectionStringMissingException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteConnectionStringMissingException"/> class.
        /// </summary>
        public SqLiteConnectionStringMissingException(IStringLocalizer<InfrastructureLayerLocalization> localizer)
            : base(localizer["SQLite connection string must be provided through 'Arguments' property in configuration file."])
        {
        }
    }
}
