using Core.Abstractions;
using Core.Extensions;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the strategy for querying all active sources.
    /// </summary>
    /// <seealso cref="ISourcesSelector" />
    public sealed class QueryAllActiveSourcesStrategy : ISourcesSelector
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly IHandlerHelper _handlerHelper;

        /// <inheritdoc cref="ISourcesSelector.QueryWithinDescription"/>
        public string QueryWithinDescription => $"all active sources";

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAllActiveSourcesStrategy"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="pluginService">The plugin service.</param>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <exception cref="System.ArgumentNullException">
        /// settingsService
        /// or
        /// pluginService
        /// or
        /// handlerHelper
        /// </exception>
        public QueryAllActiveSourcesStrategy(ISettingsService settingsService, IPluginService pluginService, IHandlerHelper handlerHelper)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        /// <inheritdoc cref="ISourcesSelector.GetSources"/>
        public IEnumerable<ISource> GetSources()
        {
            var sources = _settingsService
                .ReadSources()
                .Where(x => x.IsActive);

            return sources.ToRealInstance(_pluginService, _handlerHelper);
        }
    }
}
