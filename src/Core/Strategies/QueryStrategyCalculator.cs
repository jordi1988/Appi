using Core.Abstractions;
using Core.Models;
using Microsoft.Extensions.Localization;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the helper class that determines the correct querying strategy.
    /// </summary>
    public class QueryStrategyCalculator
    {
        private readonly ISourceService _settingsService;
        private readonly IStringLocalizer<CoreLayerLocalization> _localizer;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStrategyCalculator"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <exception cref="System.ArgumentNullException">
        /// settingsService
        /// or
        /// pluginService
        /// or
        /// handlerHelper
        /// </exception>
        public QueryStrategyCalculator(
            ISourceService settingsService,
            IServiceProvider serviceProvider,
            IStringLocalizer<CoreLayerLocalization> localizer)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        /// <summary>
        /// Creates the specific stategy based on the provided options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="queryAllDefaultValue">The default value of the query all group.</param>
        /// <returns>The concrete strategy's instance.</returns>
        public ISourceStrategy Create(FindItemsOptions options, string queryAllDefaultValue)
        {
            bool isSourceProvided = !string.IsNullOrWhiteSpace(options.SourceAlias);
            bool isGroupProvided = !queryAllDefaultValue.Equals(options.GroupAlias);

            if (isSourceProvided)
            {
                return new QuerySingleSourceStrategy(_settingsService, _serviceProvider, _localizer, options.SourceAlias!);
            }
            else if (isGroupProvided)
            {
                return new QueryGroupStrategy(_settingsService, _serviceProvider, _localizer, options.GroupAlias!);
            }

            return new QueryAllActiveSourcesStrategy(_settingsService, _serviceProvider, _localizer);
        }
    }
}
