using Core.Abstractions;
using Core.Extensions;

namespace Core.Strategies
{
    public class QueryAllActiveSourcesStrategy : ISourcesSelector
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly IHandlerHelper _handlerHelper;

        public string QueryWithinDescription => $"all active sources";

        public QueryAllActiveSourcesStrategy(ISettingsService settingsService, IPluginService pluginService, IHandlerHelper handlerHelper)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
        }

        public IEnumerable<ISource> GetSources()
        {
            var sources = _settingsService
                .ReadSettingsFileSources()
                .Where(x => x.IsActive);

            return sources.ToRealInstance(_pluginService, _handlerHelper);
        }
    }
}
