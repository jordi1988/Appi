using Core.Abstractions;
using Core.Exceptions;
using Core.Extensions;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the strategy for querying one single source.
    /// </summary>
    /// <seealso cref="Core.Abstractions.ISourcesSelector" />
    public sealed class QuerySingleSourceStrategy : ISourcesSelector
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly IHandlerHelper _handlerHelper;
        private readonly string _sourceAlias;

        /// <inheritdoc cref="ISourcesSelector.QueryWithinDescription"/>
        public string QueryWithinDescription => $"source `{_sourceAlias}`";

        /// <summary>
        /// Initializes a new instance of the <see cref="QuerySingleSourceStrategy"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="pluginService">The plugin service.</param>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <param name="sourceAlias">The source alias.</param>
        /// <exception cref="System.ArgumentNullException">
        /// settingsService
        /// or
        /// pluginService
        /// or
        /// handlerHelper
        /// </exception>
        public QuerySingleSourceStrategy(ISettingsService settingsService, IPluginService pluginService, IHandlerHelper handlerHelper, string sourceAlias)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
            _sourceAlias = sourceAlias;
        }

        /// <inheritdoc cref="ISourcesSelector.GetSources"/>
        public IEnumerable<ISource> GetSources()
        {
            var source = _settingsService
                .ReadSources()
                .FirstOrDefault(x => _sourceAlias!.Equals(x.Alias))
                    ?? throw new SourceNotFoundException($"{_sourceAlias} (Alias)");

            var output = new[] { source };

            return output.ToRealInstance(_pluginService, _handlerHelper);
        }
    }
}
