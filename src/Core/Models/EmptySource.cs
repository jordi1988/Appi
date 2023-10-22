using Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Models
{
    /// <summary>
    /// Dummy implementation of <see cref="ISource"/>.
    /// </summary>
    /// <seealso cref="ISource" />
    public sealed class EmptySource : ISource
    {
        /// <inheritdoc cref="ISource.TypeName"/>
        public string TypeName { get; set; } = typeof(EmptySource).Name;

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

        /// <inheritdoc cref="ISource.AddCustomServices"/>
        public IServiceCollection AddCustomServices(IServiceCollection services)
        {
            return services;
        }

        /// <inheritdoc cref="ISource.ReadAsync(FindItemsOptions)"/>
        /// <returns>Empty collection.</returns>
        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var emptyCollection = Enumerable.Empty<ResultItemBase>();

            return await Task.FromResult(emptyCollection);
        }
    }
}
