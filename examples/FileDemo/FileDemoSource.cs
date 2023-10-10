using Core.Abstractions;
using Core.Models;
using Infrastructure.Sources.File;

namespace Infrastructure.FileDemo
{
    public class FileDemoSource : FileSource
    {
        private readonly IHandlerHelper _handlerHelper;

        public override string TypeName { get; set; } = typeof(FileDemoSource).Name;
        public override string Name { get; set; } = "scraped.txt File";
        public override string Alias { get; set; } = "demofile";
        public override string Description { get; set; } = "Contents of the file.";
        public override bool IsActive { get; set; } = false;
        public override int SortOrder { get; set; } = 10;
        public override string? Path { get; set; } = @"E:\scraped.txt";
        public override string? Arguments { get; set; }
        public override bool? IsQueryCommand { get; set; } = true;

        // Either this constructor ...
        public FileDemoSource()
        {
        }

        // ... or this constructor
        public FileDemoSource(IHandlerHelper handlerHelper)
        {
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        public override async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            if (options is null)
            {
                return Enumerable.Empty<ResultItemBase>();
            }

            var stringComparison = options.CaseSensitive ?
                StringComparison.CurrentCulture :
                StringComparison.CurrentCultureIgnoreCase;

            var allItems = await base.ReadAsync(options);
            var queriedItems = allItems
                .Where(x => x.Description.Contains(options.Query, stringComparison));

            return queriedItems;
        }

        protected override FileResult Parse(string row, int rowNumber)
        {
            return new FileDemoResult(Path!, rowNumber, _handlerHelper)
            {
                Id = rowNumber,
                Name = $"Line {rowNumber}",
                Description = row
            };
        }
    }
}
