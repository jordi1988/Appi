using Core.Abstractions;

namespace Core.Entities
{
    public sealed class DeserializationSource : ISource
    {
        public string TypeName { get; set; } = typeof(DeserializationSource).Name;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }
        public string? Path { get; set; }

        public async Task<IEnumerable<ResultItemBase>> ReadAsync()
        {
            return await Task.FromResult(Enumerable.Empty<ResultItemBase>());
        }
    }
}
