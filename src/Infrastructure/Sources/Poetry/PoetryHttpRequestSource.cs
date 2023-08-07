using Core.Abstractions;
using Core.Helper;
using Core.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Sources.Poetry
{
    internal partial class PoetryHttpRequestSource : ISource
    {
        public string TypeName { get; set; } = typeof(PoetryHttpRequestSource).Name;
        public string Name { get; set; } = "Poetry";
        public string Description { get; set; } = "by poetrydb.org";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 20;
        public string? Path { get; set; } = $"https://poetrydb.org/title/{ConfigurationHelper.QueryParam}";

        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            ValidateConfig();

            Path = Path!.Replace(ConfigurationHelper.QueryParam, options?.Query);

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

            foreach (var title in titles)
            {
                output.Add(new()
                {
                    Author = title.Author,
                    Title = title.Title.Length <= 100 ? title.Title : title.Title[..100] + "...",
                    Lines = string.Join(" ", title.Lines),
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
    }

    internal sealed record Titles(string Author, string Title, string[] Lines, string LineCount);
}
