using Core.Abstractions;
using Core.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Sources
{
    /// <summary>
    /// Represents a base class for SQL sources.
    /// </summary>
    /// <typeparam name="TResult">The result type to be mapped.</typeparam>
    /// <seealso cref="ISource" />
    public abstract class DapperSqlSource<TResult> : ISource
        where TResult : class
    {
        /// <inheritdoc cref="ISource.TypeName"/>
        public abstract string TypeName { get; set; }

        /// <inheritdoc cref="ISource.Name"/>
        public abstract string Name { get; set; }

        /// <inheritdoc cref="ISource.Alias"/>
        public abstract string Alias { get; set; }

        /// <inheritdoc cref="ISource.Description"/>
        public abstract string Description { get; set; }

        /// <inheritdoc cref="ISource.IsActive"/>
        public abstract bool IsActive { get; set; }

        /// <inheritdoc cref="ISource.SortOrder"/>
        public abstract int SortOrder { get; set; }

        /// <inheritdoc cref="ISource.Path"/>
        public abstract string? Path { get; set; }

        /// <inheritdoc cref="ISource.Arguments"/>
        public abstract string? Arguments { get; set; }

        /// <inheritdoc cref="ISource.IsQueryCommand"/>
        public abstract bool? IsQueryCommand { get; set; }

        /// <inheritdoc cref="ISource.Groups"/>
        public string[]? Groups { get; set; }

        /// <summary>
        /// Gets the handler helper.
        /// </summary>
        /// <value>The handler helper.</value>
        public IHandlerHelper HandlerHelper { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperSqlSource{TResult}"/> class.
        /// </summary>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected DapperSqlSource(IHandlerHelper handlerHelper)
        {
            HandlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        /// <summary>
        /// Fetches the data from the SQL server.
        /// </summary>
        /// <param name="options">Options related to the query.</param>
        /// <returns>The mapped results of the query.</returns>
        public virtual async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var connectionString = Arguments ?? throw new SqlConnectionStringMissingException();

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var sqlQuery = GetSqlQuery(options);
            var results = connection.Query<TResult>(sqlQuery);
            var output = results.Select(Parse);

            return output;
        }

        /// <summary>
        /// Gets the SQL query which will be run.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The constructed <c>SELECT</c>-query</returns>
        protected abstract CommandDefinition GetSqlQuery(FindItemsOptions options);

        /// <summary>
        /// Parses the selected rows into result output objects.
        /// </summary>
        /// <param name="row">The selected row.</param>
        /// <returns>The result output object.</returns>
        protected abstract ResultItemBase Parse(TResult row);

        /// <summary>
        /// Encodes the provided search query for the use with <c>LIKE</c> constraint.
        /// </summary>
        /// <param name="value">The search query to be encoded for the use with <c>LIKE</c>.</param>
        /// <returns>The encoded search query.</returns>
        protected string EncodeForLike(string value)
        {
            return value
                .Replace("[", "[[]")
                .Replace("%", "[%]");
        }
    }
}
