using Domain.Entities;

namespace Domain.Entities
{
    public class PromptGroup
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<Result> Items { get; set; } = Enumerable.Empty<Result>();
    }
}
