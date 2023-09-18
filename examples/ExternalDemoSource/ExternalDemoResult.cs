using Core.Abstractions;
using Core.Attributes;
using Core.Models;

namespace Infrastructure.ExternalSourceDemo
{
    public class ExternalDemoResult : ResultItemBase
    {
        [Result]
        public override string Name { get => base.Name; set => base.Name = value; }

        [Result]
        public override string Description { get => base.Description; set => base.Description = value; }

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
