using Core.Abstractions;
using static Ui.Appi.Commands.FindItemsCommand;

namespace ExternalSourceDemo
{
    public class ExternalDemoSource : ISource
    {
        private readonly Settings? _settings;
        public string TypeName { get; set; } = typeof(ExternalDemoSource).Name;
        public string Name { get; set; } = "Demo Assembly";
        public string Description { get; set; } = "Returns hard-coded hello world.";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 50;
        public string? Path { get; set; } = null;

        // TODO: only one parameter FindItemsCommand.Settings allowed in ctor
        public ExternalDemoSource(Settings? settings) : base()
        {
            _settings = settings;
        }

        public async Task<IEnumerable<ResultItemBase>> ReadAsync()
        {
            var output = new List<ExternalDemoResult>()
            {
                new() { Name = "Hello", Description = _settings?.Query ?? "World" }
            };

            return await Task.FromResult(output);
        }
    }
}
