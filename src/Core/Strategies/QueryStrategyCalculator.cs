using Core.Abstractions;
using Core.Models;

namespace Core.Strategies
{
    public class QueryStrategyCalculator
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;

        public QueryStrategyCalculator(ISettingsService settingsService, IPluginService pluginService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
        }

        public ISourcesSelector Create(FindItemsOptions options, string queryAllDefaultValue)
        {
            bool isSourceProvided = !string.IsNullOrWhiteSpace(options.SourceAlias);
            bool isGroupProvided = !queryAllDefaultValue.Equals(options.GroupAlias);

            if (isSourceProvided)
            {
                return new QuerySingleSourceStrategy(_settingsService, _pluginService, options.SourceAlias!);
            }
            else if (isGroupProvided)
            {
                return new QueryGroupStrategy(_settingsService, _pluginService, options.GroupAlias!);
            }

            return new QueryAllActiveSourcesStrategy(_settingsService, _pluginService);
        }
    }
}
