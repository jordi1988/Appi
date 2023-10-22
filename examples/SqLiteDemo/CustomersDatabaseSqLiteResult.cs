using Core.Abstractions;
using Core.Attributes;
using Core.Models;
using Infrastructure.Services;
using Infrastructure.Sources.Sql;
using Microsoft.Extensions.Localization;
using TextCopy;

namespace Infrastructure.SqLite
{
    public class CustomersDatabaseSqLiteResult : SqlResultBase<CustomerDto>
    {
        private readonly IHandlerHelper _handlerHelper;
        private readonly IStringLocalizer<CustomersSqLiteSource> _customLocalizer;

        [DetailViewColumn]
        public string? FirstName => Result.FirstName;

        [DetailViewColumn]
        public string? LastName => Result.LastName;

        [DetailViewColumn]
        public string? Company => Result.Company;

        [DetailViewColumn]
        public string? Address => Result.Address;

        [DetailViewColumn]
        public string? City => Result.City;

        [DetailViewColumn]
        public string? State => Result.State;

        [DetailViewColumn]
        public string? Country => Result.Country;

        [DetailViewColumn]
        public string? PostalCode => Result.PostalCode;

        [DetailViewColumn]
        public string? Phone => Result.Phone;

        [DetailViewColumn]
        public string? Fax => Result.Fax;

        [DetailViewColumn]
        public string? Email => Result.Email;

        public CustomersDatabaseSqLiteResult(
            CustomerDto result, 
            IHandlerHelper handlerHelper, 
            IStringLocalizer<CustomersSqLiteSource> customLocalizer)
            : base(result)
        {
            base.Id = Result.CustomerId;
            base.Name = $"{FirstName} {LastName}";
            base.Description = $"{Country}, {Address}";
            base.Sort = 99;

            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
            _customLocalizer = customLocalizer ?? throw new ArgumentNullException(nameof(customLocalizer));
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            var url = $"https://www.google.com/maps/search/?api=1&query={Name}, {Description}";

            var actions = new List<ActionItem>() {
                _handlerHelper.Back(),
                new() {
                    Name = _customLocalizer["Open in Google Maps"],
                    Action = () => ProcessService.Start(url.Replace("&", "^&"))
                },
                new() {
                    Name = _customLocalizer["Copy address"],
                    Action = () => ClipboardService.SetText($"""
                        {Address}
                        {PostalCode} {City}
                        {Country}
                        """)
                },
                _handlerHelper.Exit()
            };

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                actions.Insert(3, new ActionItem()
                {
                    Name = _customLocalizer["Make phone call to {0}", Phone],
                    Action = () => ProcessService.Start($"tel:{Phone}")
                });
            }

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
