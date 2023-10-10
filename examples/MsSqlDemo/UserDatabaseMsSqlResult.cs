using Core.Abstractions;
using Core.Attributes;
using Core.Models;
using Infrastructure.Services;

namespace Infrastructure.MsSql
{
    public class UserDatabaseMsSqlResult : MsSqlResultBase<UserDto>
    {
        private readonly IHandlerHelper _handlerHelper;

        [DetailViewColumn]
        public string UserID => Result.UserID.ToString();

        [DetailViewColumn]
        public string UserName => _handlerHelper.EscapeMarkup(Result.UserName);

        public UserDatabaseMsSqlResult(UserDto result, IHandlerHelper handlerHelper)
            : base(result)
        {
            base.Id = 0;
            base.Name = result.UserName;
            base.Description = result.UserID.ToString();
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
