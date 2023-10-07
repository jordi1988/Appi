using Core.Attributes;
using Core.Models;
using Infrastructure.Services;

namespace Infrastructure.MySql
{
    public class AddressDatabaseMySqlResult : MySqlResultBase<AddressDto>
    {
        [Result]
        public string? Street => Result.Street;

        [Result]
        public int? Number => Result.Number;

        [Result]
        public string? PostalCode => Result.PostalCode;

        [Result]
        public string? City => Result.City;

        public AddressDatabaseMySqlResult(AddressDto result)
            : base(result)
        {
            base.Id = 0;
            base.Name = $"{result.Street} {result.Number}";
            base.Description = $"{result.PostalCode} {result.City}";
            base.Sort = 99;
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            var url = $"https://www.google.com/maps/search/?api=1&query={Name}, {Description}";
            var actions = new List<ActionItem>() {
                new() {
                    Name = "Open Google Maps",
                    Action = () => ProcessService.Start(url.Replace("&", "^&"))
                }
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
