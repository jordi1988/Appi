using Core.Abstractions;
using Core.Models;

namespace Core.Strategies
{
    public class QueryStrategyCalculator
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly IHandlerHelper _handlerHelper;

        public QueryStrategyCalculator(ISettingsService settingsService, IPluginService pluginService, IHandlerHelper handlerHelper)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

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
