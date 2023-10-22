using Core.Abstractions;
using Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Localization;
using System.Data.Common;

namespace Infrastructure.Sources.Sql.MsSql
{
    /// <summary>
    /// Represents a base class for SQL Server sources.
    /// </summary>
    /// <typeparam name="TResult">The result type to be mapped.</typeparam>
    /// <seealso cref="ISource" />
    public abstract class MsSqlSource<TResult> : DapperSqlSource<TResult>
        where TResult : class
    {
        private readonly IStringLocalizer<InfrastructureLayerLocalization> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsSqlSource{TResult}"/> class.
        /// </summary>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <param name="localizer">The localizer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected MsSqlSource(IHandlerHelper handlerHelper, IStringLocalizer<InfrastructureLayerLocalization> localizer)
            : base(handlerHelper)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Fetches the data from the SQL Server.
        /// </summary>
        /// <param name="options">Options related to the query.</param>
        /// <returns>The mapped results of the query.</returns>
        public override async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            if (Arguments is null)
            {
                throw new MsSqlConnectionStringMissingException(_localizer);
            }

            return await base.ReadAsync(options);
        }

        /// <summary>
        /// Creates the SQL server database connection passing the connection string.
        /// </summary>
        /// <returns>
        /// Instance of SqlConnection
        /// </returns>
        protected override DbConnection CreateConnection()
        {
            return new SqlConnection(Arguments);
        }
    }
}
