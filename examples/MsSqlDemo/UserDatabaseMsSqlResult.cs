using Core.Abstractions;
using Core.Attributes;
using Core.Models;
using Infrastructure.Sources.Sql;

namespace Infrastructure.MsSql
{
    public class UserDatabaseMsSqlResult : SqlResultBase<UserDto>
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
            var actions = new List<ActionItem>() {
                _handlerHelper.Back(),
                new() {
                    Name = "Edit user in ADSI Edit",
                    Action = () => Console.WriteLine("Sorry, just a demo.")
                },
                new() {
                    Name = "Delete user",
                    Action = () => Console.WriteLine("Sorry, just a demo.")
                },
                new() {
                    Name = "Lock user",
                    Action = () => Console.WriteLine("Sorry, just a demo.")
                },
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
