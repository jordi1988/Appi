using Core.Abstractions;
using Core.Models;
using Dapper;
using Microsoft.Extensions.Localization;

namespace Infrastructure.MySql
{
    public class AddressMySqlSource : MySqlSource<AddressDto>
    {
        public override string TypeName { get; set; } = typeof(AddressMySqlSource).Name;
        public override string Name { get; set; } = "Query address MySQL Database";
        public override string Alias { get; set; } = "address";
        public override string Description { get; set; } = "Lookup addresses.";
        public override bool IsActive { get; set; } = true;
        public override int SortOrder { get; set; } = 30;
        public override string? Path { get; set; }
        public override string? Arguments { get; set; } = "INSERT_CONNECTIONSTRING_HERE";
        public override bool? IsQueryCommand { get; set; } = true;

        public AddressMySqlSource(IHandlerHelper handlerHelper, IStringLocalizer<InfrastructureLayerLocalization> localizer)
            : base(handlerHelper, localizer)
        {
        }

        protected override CommandDefinition GetSqlQuery(FindItemsOptions options)
        {
            var query = $"%{EncodeForLike(options.Query)}%";

            return new CommandDefinition(
                commandText: $"SELECT * FROM Address WHERE Street LIKE @query",
                parameters: new { query });
        }

        protected override ResultItemBase Parse(AddressDto row)
        {
            return new AddressDatabaseMySqlResult(row, HandlerHelper);
        }
    }
}
