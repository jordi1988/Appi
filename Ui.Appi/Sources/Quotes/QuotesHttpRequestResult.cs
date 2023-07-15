using Domain.Entities;

namespace Infrastructure.Sources.HttpRequest
{
    internal class QuotesHttpRequestResult : Result
    {
        public override string Name { get => Author; set => Author = value; }
        public override string Description { get => Content; set => Content = value; }
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateOnly DateAdded { get; set; }
        public DateOnly DateModified { get; set; }

        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { Name = $"Quit", Action = () => { Console.WriteLine($"Goodbye."); } },
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,35}";
        }
    }
}
