using Core.Abstractions;
using Core.Models;

namespace Infrastructure.ExternalSourceDemo
{
    public class ExternalDemoSource : ISource
    {
        private readonly IHandlerHelper? _handlerHelper;

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

        // Either this constructor ...
        public ExternalDemoSource()
        {
        }

        // ... or this constructor
        public ExternalDemoSource(IHandlerHelper handlerHelper)
        {
            _handlerHelper = handlerHelper;
        }

        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var output = new List<ExternalDemoResult>()
            {
                new ExternalDemoResult(_handlerHelper) {
                    Name = "Hello",
                    Description = options?.Query ?? "World"
                }
            };

            return await Task.FromResult(output);
        }
    }
}
