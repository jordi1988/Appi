using Core.Abstractions;
using Infrastructure.Sources.File;
using static Ui.Appi.Commands.FindItemsCommand;

namespace Ui.Appi.Sources.DemoFile
{
    internal class DemoFileSource : FileSource
    {
        private readonly Settings? _settings;
        public override string TypeName { get; set; } = typeof(DemoFileSource).Name;
        public override string Name { get; set; } = "scraped.txt File";
        public override string Description { get; set; } = "Contents of the file.";
        public override bool IsActive { get; set; } = true;
        public override int SortOrder { get; set; } = 10;
        public override string? Path { get; set; } = @"E:\scraped.txt";

        public DemoFileSource(Settings? settings) : base()
        {
            _settings = settings;
        }

        public override async Task<IEnumerable<ResultItemBase>> ReadAsync()
        {
            if (_settings is null)
            {
                return Enumerable.Empty<ResultItemBase>();
            }

            var stringComparison = _settings.CaseSensitive ?
                StringComparison.CurrentCulture :
                StringComparison.CurrentCultureIgnoreCase;

            var allItems = await base.ReadAsync();
            var queriedItems = allItems
                .Where(x => x.Description.Contains(_settings.Query, stringComparison));

            return queriedItems;
        }

        protected override FileResult Parse(string row, int rowNumber)
        {
            return new DemoFileResult(Path!, rowNumber)
            {
                Id = rowNumber,
                Name = $"Line {rowNumber}",
                Description = row
            };
        }
    }
}
