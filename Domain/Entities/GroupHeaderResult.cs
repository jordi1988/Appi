namespace Domain.Entities
{
    public class GroupHeaderResult : Result
    {
        public GroupHeaderResult(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            // header cannot be selected
            return Enumerable.Empty<ActionItem>();
        }

        public override string ToString()
        {
            return $"{Name} ({Description})";
        }
    }
}
