using Core.Abstractions;
using Core.Models;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the helper class that determines the correct querying strategy.
    /// </summary>
    public class QueryStrategyCalculator
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly IHandlerHelper _handlerHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStrategyCalculator"/> class.
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
        public QueryStrategyCalculator(ISettingsService settingsService, IPluginService pluginService, IHandlerHelper handlerHelper)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        /// <summary>
        /// Creates the specific stategy based on the provided options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="queryAllDefaultValue">The default value of the query all group.</param>
        /// <returns>The concrete strategy's instance.</returns>
        public ISourcesSelector Create(FindItemsOptions options, string queryAllDefaultValue)
        {
            bool isSourceProvided = !string.IsNullOrWhiteSpace(options.SourceAlias);
            bool isGroupProvided = !queryAllDefaultValue.Equals(options.GroupAlias);

            if (isSourceProvided)
            {
                return new QuerySingleSourceStrategy(_settingsService, _pluginService, _handlerHelper, options.SourceAlias!);
            }
            else if (isGroupProvided)
            {
                return new QueryGroupStrategy(_settingsService, _pluginService, _handlerHelper, options.GroupAlias!);
            }

            return new QueryAllActiveSourcesStrategy(_settingsService, _pluginService, _handlerHelper);
        }
    }
}
