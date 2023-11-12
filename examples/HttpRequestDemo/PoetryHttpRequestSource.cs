using ArgumentStringNS;
using Core.Abstractions;
using Core.Helper;
using Core.Models;
using Microsoft.Extensions.Localization;
using System.Net.Http.Json;
using System.Text.Json;

[assembly: RootNamespace("HttpRequestDemo")]

namespace Infrastructure.HttpRequestDemoExample
{
    internal partial class PoetryHttpRequestSource : ISource
    {
        private readonly IStringLocalizer<PoetryHttpRequestSource> _customLocalizer;
        private readonly IHandlerHelper _handlerHelper;
        private const string _queryParam = "##QUERY##";

        public string TypeName { get; set; } = typeof(PoetryHttpRequestSource).Name;
        public string Name { get; set; } = "Poetry";
        public string Alias { get; set; } = "poetry";
        public string Description { get; set; } = "by poetrydb.org";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 20;
        public string? Path { get; set; } = $"https://poetrydb.org/title/{_queryParam}";
        public string? Arguments { get; set; } = "max_title_length=50";
        public bool? IsQueryCommand { get; set; } = true;
        public string[]? Groups { get; set; } = new[] { "demo" };

        public PoetryHttpRequestSource(IStringLocalizer<PoetryHttpRequestSource> customLocalizer, IHandlerHelper handlerHelper)
        {
            _customLocalizer = customLocalizer ?? throw new ArgumentNullException(nameof(customLocalizer));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        /// <summary>
        /// Reads the asynchronous.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <remarks>This code should be optimized, but it is for demo purposes only...</remarks>
        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            ValidateConfig();

            Path = Path!.Replace(_queryParam, options?.Query);

            using var client = new HttpClient();
            var output = new List<PoetryHttpRequestResult>();

            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            var response = await client.GetAsync(Path);

            var content = response?.Content.ReadAsStringAsync().Result;
            if (response is null || content!.Contains("404"))
            {
                return Enumerable.Empty<ResultItemBase>();
            }

            var titles = await response.Content.ReadFromJsonAsync<Titles[]>(serializerOptions);
            if (titles is null)
            {
                return Enumerable.Empty<ResultItemBase>();
            }

            int maxTitleLength = ReadMaxTitleLength();
            foreach (var title in titles)
            {
                output.Add(new PoetryHttpRequestResult(_customLocalizer, _handlerHelper)
                {
                    Author = title.Author,
                    Title = title.Title.Length <= maxTitleLength ? title.Title : title.Title[..maxTitleLength] + "...",
                    Lines = string.Join(" ", title.Lines),
                    LineCount = title.Lines.Length,
                    Sort = title.Title.Length,
                });
            }

            return output;
        }

        private void ValidateConfig()
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                throw new ArgumentException(nameof(Path));
            }
        }

        private int ReadMaxTitleLength()
        {
            var arguments = new ArgumentString(Arguments ?? string.Empty);
            _ = int.TryParse(arguments["max_title_length"], out int maxTitleLength);

            if (maxTitleLength == 0)
            {
                return 100;
            }

            return maxTitleLength;
        }
    }

    internal sealed record Titles(string Author, string Title, string[] Lines, string LineCount);
}
