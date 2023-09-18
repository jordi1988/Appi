using Core.Abstractions;
using Core.Models;
using Dapper;

namespace Infrastructure.MsSql
{
    public class UserMsSqlSource : MsSqlSource<UserDto>
    {
        public override string TypeName { get; set; } = typeof(UserMsSqlSource).Name;
        public override string Name { get; set; } = "Query user SQL Server Database";
        public override string Alias { get; set; } = "user";
        public override string Description { get; set; } = "Lookup users.";
        public override bool IsActive { get; set; } = true;
        public override int SortOrder { get; set; } = 30;
        public override string? Path { get; set; }
        public override string? Arguments { get; set; } = "INSERT_CONNECTIONSTRING_HERE";
        public override bool? IsQueryCommand { get; set; } = true;

        protected override CommandDefinition GetSqlQuery(FindItemsOptions options)
        {
            var query = $"%{EncodeForLike(options.Query)}%";

            return new CommandDefinition(
                commandText: $"SELECT * FROM Users WHERE UserName LIKE @query",
                parameters: new { query });
        }

        protected override ResultItemBase Parse(UserDto row)
        {
            return new UserDatabaseMsSqlResult(row);
        }
    }
}
