using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Sources.HttpRequest;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ui.Appi.Helper;
using static Ui.Appi.Commands.FindItemsCommand;

namespace Ui.Appi.Sources.Poetry
{
    internal partial class PoetryHttpRequestSource : ISource
    {
        private readonly Settings? _settings;

        public string TypeName { get; set; } = typeof(PoetryHttpRequestSource).Name;
        public string Name { get; set; } = "Poetry";
        public string Description { get; set; } = "by poetrydb.org";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 20;
        public string? Path { get; set; } = $"https://poetrydb.org/title/{ConfigurationHelper.QueryParam}";

        public PoetryHttpRequestSource(Settings? settings) : base()
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Result>> ReadAsync()
        {
            ValidateConfig();

            Path = Path!.Replace(ConfigurationHelper.QueryParam, _settings?.Query);

            using var client = new HttpClient();

            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var response = await client.GetFromJsonAsync<Titles[]>(Path, serializerOptions);

            var output = new List<PoetryHttpRequestResult>();
            if (response is null)
            {
                return Enumerable.Empty<Result>();
            }

            foreach (var item in response)
            {
                output.Add(new()
                {
                    Author = item.Author,
                    Title = item.Title.Length <= 100 ? item.Title : item.Title[..100] + "...",
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

    internal sealed record Titles(string Author, string Title, string[] Lines, string LineCount);
}
