using Domain.Entities;

namespace ExternalSourceDemo
{
    public class ExternalDemoResult : Result
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
