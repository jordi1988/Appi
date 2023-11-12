using Core.Abstractions;
using Core.Extensions;
using Microsoft.Extensions.Localization;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the strategy for querying all active sources.
    /// </summary>
    /// <seealso cref="ISourceStrategy" />
    public sealed class QueryAllActiveSourcesStrategy : ISourceStrategy
    {
        private readonly ISourceService _settingsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<CoreLayerLocalization> _localizer;

        /// <inheritdoc cref="ISourceStrategy.QueryWithinDescription"/>
        public string QueryWithinDescription => _localizer["all active sources"];

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAllActiveSourcesStrategy"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <param name="localizer">The localizer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// settingsService
        /// or
        /// pluginService
        /// or
        /// handlerHelper
        /// </exception>
        public QueryAllActiveSourcesStrategy(
            ISourceService settingsService,
            IServiceProvider serviceProvider,
            IStringLocalizer<CoreLayerLocalization> localizer)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <inheritdoc cref="ISourceStrategy.GetSources"/>
        public IEnumerable<ISource> GetSources()
        {
            var sources = _settingsService
                .ReadSources()
                .Where(x => x.IsActive);

            return sources.ToRealInstance(_serviceProvider);
        }
    }
}
