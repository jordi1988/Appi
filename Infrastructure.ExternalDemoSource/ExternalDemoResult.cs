using Core.Entities;
using Core.Abstractions;

namespace ExternalSourceDemo
{
    public class ExternalDemoResult : ResultItemBase
    {
        public override IEnumerable<ActionItem> GetActions()
        {
            return Enumerable.Empty<ActionItem>();
        }

        public override string ToString()
        {
            return $"{Name} {Description}!";
        }
    }
}
