using Core.Abstractions;
using Core.Models;

namespace ExternalSourceDemo
{
    public class ExternalDemoSource : ISource
    {
        public string TypeName { get; set; } = typeof(ExternalDemoSource).Name;
        public string Name { get; set; } = "Demo Assembly";
        public string Alias { get; set; } = "external";
        public string Description { get; set; } = "Returns hard-coded hello world.";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 50;
        public string? Path { get; set; }
        public string? Arguments { get; set; }
        public bool? IsQueryCommand { get; set; } = true;
        public string[]? Groups { get; set; } = new[] { "demo" };

        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var output = new List<ExternalDemoResult>()
            {
                new() { Name = "Hello", Description = options?.Query ?? "World" }
            };

            return await Task.FromResult(output);
        }
    }
}
