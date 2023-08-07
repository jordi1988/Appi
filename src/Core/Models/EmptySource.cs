using Core.Abstractions;

namespace Core.Models
{
    public sealed class EmptySource : ISource
    {
        public string TypeName { get; set; } = typeof(EmptySource).Name;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string? Path { get; set; }

        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var emptyCollection = Enumerable.Empty<ResultItemBase>();

            return await Task.FromResult(emptyCollection);
        }
    }
}
