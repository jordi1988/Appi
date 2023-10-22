using Core.Abstractions;
using Core.Models;
using FileDemo;
using Infrastructure.Sources.File;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: RootNamespace("FileDemo")]

namespace Infrastructure.FileDemoExample
{
    public class FileDemoSource : FileSource
    {
        private readonly IHandlerHelper _handlerHelper;
        private readonly IStringLocalizer<FileDemoSource> _customLocalizer;

        public override string TypeName { get; set; } = typeof(FileDemoSource).Name;
        public override string Name { get; set; } = "scraped.txt File";
        public override string Alias { get; set; } = "demofile";
        public override string Description { get; set; } = "Contents of the file.";
        public override bool IsActive { get; set; } = false;
        public override int SortOrder { get; set; } = 10;
        public override string? Path { get; set; } = @"E:\scraped.txt";
        public override string? Arguments { get; set; }
        public override bool? IsQueryCommand { get; set; } = true;

        public FileDemoSource(
            IHandlerHelper handlerHelper,
            IStringLocalizer<InfrastructureLayerLocalization> localizer,
            IStringLocalizer<FileDemoSource> customLocalizer,
            ILogger<FileDemoSource> logger,
            DummyService? dummyService)
            : base(localizer)
        {
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
            _customLocalizer = customLocalizer ?? throw new ArgumentNullException(nameof(customLocalizer));

            Console.WriteLine(dummyService?.GetDummyServiceCreationTime());
            Task.Delay(2000).Wait();

            logger.LogInformation($"{nameof(FileDemoSource)} was sucessfully created.");
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

        public override IServiceCollection AddCustomServices(IServiceCollection services)
        {
            services.TryAddScoped<DummyService>();

            return base.AddCustomServices(services);
        }

        protected override FileResult Parse(string row, int rowNumber)
        {
            return new FileDemoResult(Path!, rowNumber, _handlerHelper, _customLocalizer)
            {
                Id = rowNumber,
                Name = $"{_customLocalizer["Line"]} {rowNumber}",
                Description = row
            };
        }
    }
}
