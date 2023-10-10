using Core.Abstractions;
using Core.Attributes;
using Core.Models;
using Infrastructure.Services;

namespace Infrastructure.MySql
{
    public class AddressDatabaseMySqlResult : MySqlResultBase<AddressDto>
    {
        private readonly IHandlerHelper _handlerHelper;

        [DetailViewColumn]
        public string? Street => Result.Street;

        [DetailViewColumn]
        public int? Number => Result.Number;

        [DetailViewColumn]
        public string? PostalCode => Result.PostalCode;

        [DetailViewColumn]
        public string? City => Result.City;

        public AddressDatabaseMySqlResult(AddressDto result, IHandlerHelper handlerHelper)
            : base(result)
        {
            base.Id = 0;
            base.Name = $"{result.Street} {result.Number}";
            base.Description = $"{result.PostalCode} {result.City}";
            base.Sort = 99;

            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            var url = $"https://www.google.com/maps/search/?api=1&query={Name}, {Description}";
            var actions = new List<ActionItem>() {
                new() {
                    Name = "Open Google Maps",
                    Action = () => ProcessService.Start(url.Replace("&", "^&"))
                },
                _handlerHelper.Back(),
                _handlerHelper.Exit()
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
