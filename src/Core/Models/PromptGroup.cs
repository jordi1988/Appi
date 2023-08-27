using Core.Abstractions;

namespace Core.Models
{
    public class PromptGroup
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<ResultItemBase> Items { get; set; } = Enumerable.Empty<ResultItemBase>();

        public PromptGroup(string name, string description, IEnumerable<ResultItemBase> items)
        {
            Name = name;
            Description = description;
            Items = items;
        }
    }
}
