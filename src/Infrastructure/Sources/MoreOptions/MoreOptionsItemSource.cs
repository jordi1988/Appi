using Core.Abstractions;
using Core.Models;

namespace Infrastructure.Sources.MoreOptions
{
    internal class MoreOptionsItemSource : ISource
    {
        public string TypeName { get; set; } = typeof(MoreOptionsItemSource).Name;
        public string Name { get; set; } = "More";
        public string Alias { get; set; } = "more";
        public string Description { get; set; } = "Non-contextual options";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 99;
        public string? Path { get; set; }
        public string? Arguments { get; set; }

        public virtual async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var output = new List<ResultItemBase>
            {
                new MoreOptionsItemResult()
                {
                    Name = "More options",
                    Description = string.Empty
                }
            };

            return await Task.FromResult(output);
        }
    }
}
