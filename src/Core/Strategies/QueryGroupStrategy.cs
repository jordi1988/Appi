using Core.Abstractions;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.Extensions.Localization;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the strategy for querying all sources of a given group.
    /// </summary>
    /// <seealso cref="ISourceStrategy" />
    public sealed class QueryGroupStrategy : ISourceStrategy
    {
        private readonly ISourceService _settingsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<CoreLayerLocalization> _localizer;
        private readonly string _groupAlias;

        /// <inheritdoc cref="ISourceStrategy.QueryWithinDescription"/>
        public string QueryWithinDescription => $"{_localizer["group"]} '{_groupAlias}'";

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryGroupStrategy"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <param name="groupAlias">The group alias.</param>
        /// <param name="localizer">The localizer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// settingsService
        /// or
        /// pluginService
        /// or
        /// handlerHelper
        /// </exception>
        public QueryGroupStrategy(
            ISourceService settingsService,
            IServiceProvider serviceProvider,
            IStringLocalizer<CoreLayerLocalization> localizer,
            string groupAlias)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _groupAlias = groupAlias;
        }

        /// <inheritdoc cref="ISourceStrategy.GetSources"/>
        public IEnumerable<ISource> GetSources()
        {
            ArgumentException.ThrowIfNullOrEmpty(_groupAlias);

            var output = _settingsService
                .ReadSources()
                .Where(x => x.Groups?.Contains(_groupAlias) ?? false);

            if (!output.Any())
            {
                throw new GroupNotFoundException(_groupAlias, _localizer);
            }

            return output.ToRealInstance(_serviceProvider);
        }
    }
}
