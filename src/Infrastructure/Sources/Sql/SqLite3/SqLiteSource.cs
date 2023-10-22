using Core.Abstractions;
using Core.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Localization;
using SQLitePCL;
using System.Data.Common;

namespace Infrastructure.Sources.Sql.SqLite3
{
    /// <summary>
    /// Represents a base class for MySQL server sources.
    /// </summary>
    /// <typeparam name="TResult">The result type to be mapped.</typeparam>
    /// <seealso cref="ISource" />
    public abstract class SqLiteSource<TResult> : DapperSqlSource<TResult>
        where TResult : class
    {
        private readonly IStringLocalizer<InfrastructureLayerLocalization> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqLiteSource{TResult}"/> class.
        /// </summary>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <param name="localizer">The localizer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected SqLiteSource(IHandlerHelper handlerHelper, IStringLocalizer<InfrastructureLayerLocalization> localizer)
            : base(handlerHelper)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Fetches the data from the MySQL server.
        /// </summary>
        /// <param name="options">Options related to the query.</param>
        /// <returns>The mapped results of the query.</returns>
        public override async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            if (Arguments is null)
            {
                throw new SqLiteConnectionStringMissingException(_localizer);
            }

            return await base.ReadAsync(options);
        }

        /// <summary>
        /// Creates the MySQL database connection passing the connection string.
        /// </summary>
        /// <returns>
        /// Instance of MySqlConnection
        /// </returns>
        protected override DbConnection CreateConnection()
        {
            SQLitePCL.raw.SetProvider(new SQLite3Provider_e_sqlite3());

            return new SqliteConnection(Arguments);
        }
    }
}
