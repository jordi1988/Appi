using Core.Abstractions;
using Core.Models;
using Dapper;
using Infrastructure.Sources.Sql.SqLite3;
using Microsoft.Extensions.Localization;

[assembly: RootNamespace("SqLiteDemo")]

namespace Infrastructure.SqLite
{
    public class CustomersSqLiteSource : SqLiteSource<CustomerDto>
    {
        private readonly IStringLocalizer<CustomersSqLiteSource> _customLocalizer;

        public override string TypeName { get; set; } = typeof(CustomersSqLiteSource).Name;
        public override string Name { get; set; } = "Query customers SQLite Database";
        public override string Alias { get; set; } = "cust";
        public override string Description { get; set; } = "Lookup customers.";
        public override bool IsActive { get; set; } = true;
        public override int SortOrder { get; set; } = 30;
        public override string? Path { get; set; }
        public override string? Arguments { get; set; } = "INSERT_CONNECTIONSTRING_HERE";
        public override bool? IsQueryCommand { get; set; } = true;

        public CustomersSqLiteSource(
            IHandlerHelper handlerHelper, 
            IStringLocalizer<InfrastructureLayerLocalization> localizer,
            IStringLocalizer<CustomersSqLiteSource> customLocalizer)
            : base(handlerHelper, localizer)
        {
            _customLocalizer = customLocalizer ?? throw new ArgumentNullException(nameof(customLocalizer));
        }

        protected override CommandDefinition GetSqlQuery(FindItemsOptions options)
        {
            var query = $"%{EncodeForLike(options.Query)}%";

            return new CommandDefinition(
                commandText: $"""
                    SELECT * 
                    FROM customers 
                    WHERE FirstName LIKE @query OR
                          LastName LIKE @query OR
                          Company LIKE @query OR
                          Address LIKE @query
                    """,
                parameters: new { query });
        }

        protected override ResultItemBase Parse(CustomerDto row)
        {
            return new CustomersDatabaseSqLiteResult(row, HandlerHelper, _customLocalizer);
        }
    }
}
