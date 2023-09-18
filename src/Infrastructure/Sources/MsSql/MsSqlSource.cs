using Core.Abstractions;
using Core.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.MsSql
{
    public abstract class MsSqlSource<TResult> : ISource
        where TResult : class
    {
        public abstract string TypeName { get; set; }
        public abstract string Name { get; set; }
        public abstract string Alias { get; set; }
        public abstract string Description { get; set; }
        public abstract bool IsActive { get; set; }
        public abstract int SortOrder { get; set; }
        public abstract string? Path { get; set; }
        public abstract string? Arguments { get; set; }
        public abstract bool? IsQueryCommand { get; set; }
        public string[]? Groups { get; set; }

        public virtual async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var connectionString = Arguments ?? throw new MsSqlConnectionStringMissingException();

            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var sqlQuery = GetSqlQuery(options);
            var results = connection.Query<TResult>(sqlQuery);
            var output = results.Select(Parse);

            return output;
        }

        protected abstract CommandDefinition GetSqlQuery(FindItemsOptions options);

        protected abstract ResultItemBase Parse(TResult row);

        protected string EncodeForLike(string value)
        {
            return value
                .Replace("[", "[[]")
                .Replace("%", "[%]");
        }
    }
}
