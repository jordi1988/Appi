using Core.Abstractions;
using Core.Attributes;
using Core.Entities;
using TextCopy;

namespace Infrastructure.Sources.HttpRequest
{
    internal class PoetryHttpRequestResult : ResultItemBase
    {
        public override string Name { get => Author; set => Author = value; }

        public override string Description { get => Title; set => Title = value; }

        [Result]
        public string Author { get; set; } = string.Empty;

        [Result]
        public string Title { get; set; } = string.Empty;

        [Result]
        public string Lines { get; set; } = string.Empty;

        public string LineCount { get; set; } = string.Empty;

        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                new() { Name = $"Copy lines to clipboard", Action = () => { ClipboardService.SetText(string.Join("\r\n", Lines)); } },
                new() { Name = $"Quit", Action = () => { Console.WriteLine($"Goodbye."); } },
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
