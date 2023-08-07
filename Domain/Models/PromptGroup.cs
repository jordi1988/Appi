using Core.Abstractions;

namespace Core.Models
{
    public class PromptGroup
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<ResultItemBase> Items { get; set; } = Enumerable.Empty<ResultItemBase>();
    }
}
