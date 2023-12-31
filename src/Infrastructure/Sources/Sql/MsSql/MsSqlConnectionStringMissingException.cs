﻿using Core.Exceptions;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Sources.Sql.MsSql
{
    /// <summary>
    /// Represents the <see cref="MsSqlConnectionStringMissingException"/> class which will be used if the connection string was not provided.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class MsSqlConnectionStringMissingException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlConnectionStringMissingException"/> class.
        /// </summary>
        public MsSqlConnectionStringMissingException(IStringLocalizer<InfrastructureLayerLocalization> localizer)
            : base(localizer["Microsoft SQL connection string must be provided through 'Arguments' property in configuration file."])
        {
        }
    }
}
