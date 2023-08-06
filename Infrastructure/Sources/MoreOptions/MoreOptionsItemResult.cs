using Core.Entities;
using Core.Abstractions;

namespace Infrastructure.Sources.MoreOptions
{
    public class MoreOptionsItemResult : ResultItemBase
    {
        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { Name = $"Quit", Action = () => { Console.WriteLine("Goodbye."); } }
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
