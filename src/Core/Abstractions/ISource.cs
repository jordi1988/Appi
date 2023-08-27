using Core.Models;

namespace Core.Abstractions
{
    public interface ISource
    {
        string TypeName { get; set; }
        string Name { get; set; }
        string Alias { get; set; }
        string[]? Groups { get; set; }
        string Description { get; set; }
        // should better be called IsIncludedInQueryAll
        bool IsActive { get; set; }
        int SortOrder { get; set; }
        string? Path { get; set; }
        string? Arguments { get; set; }
        bool? IsQueryCommand { get; set; }

        Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options);
    }
}
