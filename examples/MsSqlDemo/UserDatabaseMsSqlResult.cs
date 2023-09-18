using Core.Models;
using Infrastructure.Services;

namespace Infrastructure.MsSql
{
    public class UserDatabaseMsSqlResult : MsSqlResultBase<UserDto>
    {
        public UserDatabaseMsSqlResult(UserDto result)
            : base(result)
        {
            base.Id = 0;
            base.Name = result.UserName;
            base.Description = result.UserID.ToString();
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
