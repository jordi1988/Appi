using Domain.Entities;

namespace Ui.Appi.Sources.MoreOptions
{
    public class MoreOptionsItemResult : Result
    {
        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { Name = $"Create new item", Action = () => { Console.WriteLine($"New item will be created..."); } },
                new() { Name = $"Quit", Action = () => { Console.WriteLine("Goodbye :)"); } }
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,35}";
        }
    }
}
