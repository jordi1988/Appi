using Core.Abstractions;
using Core.Models;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Sources.MoreOptions
{
    /// <summary>
    /// Represents a non-context source.
    /// </summary>
    /// <seealso cref="ISource" />
    internal class MoreOptionsItemSource : ISource
    {
        private readonly IHandlerHelper _handlerHelper;
        private readonly IStringLocalizer<InfrastructureLayerLocalization> _localizer;

        /// <inheritdoc cref="ISource.TypeName"/>
        public string TypeName { get; set; } = typeof(MoreOptionsItemSource).Name;

        /// <inheritdoc cref="ISource.Name"/>
        public string Name { get; set; } = "More";

        /// <inheritdoc cref="ISource.Alias"/>
        public string Alias { get; set; } = "more";

        /// <inheritdoc cref="ISource.Description"/>
        public string Description { get; set; } = "Non-contextual options";

        /// <inheritdoc cref="ISource.IsActive"/>
        public bool IsActive { get; set; } = true;

        /// <inheritdoc cref="ISource.SortOrder"/>
        public int SortOrder { get; set; } = 99;

        /// <inheritdoc cref="ISource.Path"/>
        public string? Path { get; set; }

        /// <inheritdoc cref="ISource.Arguments"/>
        public string? Arguments { get; set; }

        /// <inheritdoc cref="ISource.IsQueryCommand"/>
        public bool? IsQueryCommand { get; set; } = false;

        /// <inheritdoc cref="ISource.Groups"/>
        public string[]? Groups { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreOptionsItemSource"/> class.
        /// </summary>
        /// <remarks>This constructor exists for reading the instance's properties and should not be called directly.</remarks>
        public MoreOptionsItemSource()
        {
            _handlerHelper = null!;
            _localizer = null!;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreOptionsItemSource"/> class.
        /// </summary>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <param name="localizer">The localizer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MoreOptionsItemSource(IHandlerHelper handlerHelper, IStringLocalizer<InfrastructureLayerLocalization> localizer)
        {
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Returns hard coded non-contextual items.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>Hard coded non-contextual items</returns>
        public virtual async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var output = new List<ResultItemBase>
            {
                new MoreOptionsItemResult(_handlerHelper, _localizer)
            };

            return await Task.FromResult(output);
        }
    }
}
