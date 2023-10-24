using Core.Abstractions;
using System.Diagnostics;

namespace Core.Models
{
    /// <summary>
    /// Dummy implementation of <see cref="ISource"/> for reading and writing the settings file.
    /// </summary>
    /// <seealso cref="ISource" />
    [DebuggerDisplay("{TypeName}")]
    public sealed class JsonFileSource : ISource
    {
        /// <inheritdoc cref="ISource.TypeName"/>
        public string TypeName { get; set; } = typeof(JsonFileSource).Name;

        /// <inheritdoc cref="ISource.Name"/>
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc cref="ISource.Alias"/>
        public string Alias { get; set; } = string.Empty;

        /// <inheritdoc cref="ISource.Description"/>
        public string Description { get; set; } = string.Empty;

        /// <inheritdoc cref="ISource.IsActive"/>
        public bool IsActive { get; set; }

        /// <inheritdoc cref="ISource.SortOrder"/>
        public int SortOrder { get; set; }

        /// <inheritdoc cref="ISource.Path"/>
        public string? Path { get; set; }

        /// <inheritdoc cref="ISource.Arguments"/>
        public string? Arguments { get; set; }

        /// <inheritdoc cref="ISource.IsQueryCommand"/>
        public bool? IsQueryCommand { get; set; }

        /// <inheritdoc cref="ISource.Groups"/>
        public string[]? Groups { get; set; }

        /// <inheritdoc cref="ISource.ReadAsync(FindItemsOptions)"/>
        /// <returns>Empty collection.</returns>
        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var emptyCollection = Enumerable.Empty<ResultItemBase>();

            return await Task.FromResult(emptyCollection);
        }
    }
}
