using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Sources.HttpRequest;
using System.Net.Http.Json;
using System.Text.Json;
using Ui.Appi.Helper;
using static Ui.Appi.Commands.FindItemsCommand;

namespace Ui.Appi.Sources.Quotes
{
    internal partial class QuotesHttpRequestSource : ISource
    {
        private readonly Settings? _settings;

        public string TypeName { get; set; } = typeof(QuotesHttpRequestSource).Name;
        public string Name { get; set; } = "Quotes";
        public string Description { get; set; } = "Quotes by quatable.io";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 20;
        public string? Path { get; set; } = $"https://api.quotable.io/search/quotes?query={ConfigurationHelper.QueryParam}&limit=150&fuzzyMaxEdits=1";

        public QuotesHttpRequestSource(Settings? settings) : base()
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Result>> ReadAsync()
        {
            ValidateConfig();

            Path = Path!.Replace(ConfigurationHelper.QueryParam, _settings?.Query);

            using var client = new HttpClient();

            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var response = await client.GetFromJsonAsync<QuotesResponse>(Path, serializerOptions);

            var output = new List<QuotesHttpRequestResult>();
            if (response?.Results is null)
            {
                return Enumerable.Empty<Result>();
            }

            foreach (var item in response.Results)
            {
                output.Add(new()
                {
                    Author = item.Author,
                    Content = item.Content.Length <= 100 ? item.Content : item.Content[..100] + "...",
                    DateAdded = item.DateAdded,
                    DateModified = item.DateModified
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
    }
}
