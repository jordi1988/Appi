using Core.Abstractions;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.Extensions.Localization;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the strategy for querying one single source.
    /// </summary>
    /// <seealso cref="Core.Abstractions.ISourcesSelector" />
    public sealed class QuerySingleSourceStrategy : ISourcesSelector
    {
        private readonly ISettingsService _settingsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<CoreLayerLocalization> _localizer;
        private readonly string _sourceAlias;

        /// <inheritdoc cref="ISourcesSelector.QueryWithinDescription"/>
        public string QueryWithinDescription => $"{_localizer["source"]} '{_sourceAlias}'";

        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySingleSourceStrategy"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="sourceAlias">The source alias.</param>
        /// <exception cref="System.ArgumentNullException">
        /// settingsService
        /// or
        /// pluginService
        /// or
        /// handlerHelper
        /// </exception>
        public QuerySingleSourceStrategy(
            ISettingsService settingsService, 
            IServiceProvider serviceProvider,
            IStringLocalizer<CoreLayerLocalization> localizer, 
            string sourceAlias)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _sourceAlias = sourceAlias;
        }

        /// <inheritdoc cref="ISourcesSelector.GetSources"/>
        public IEnumerable<ISource> GetSources()
        {
            var source = _settingsService
                .ReadSources()
                .FirstOrDefault(x => _sourceAlias!.Equals(x.Alias))
                    ?? throw new SourceNotFoundException($"{_sourceAlias} ({_localizer["alias"]})", _localizer);

            var output = new[] { source };

            return output.ToRealInstance(_serviceProvider);
        }
    }
}
