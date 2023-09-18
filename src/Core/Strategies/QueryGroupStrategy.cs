using Core.Abstractions;
using Core.Exceptions;
using Core.Extensions;

namespace Core.Strategies
{
    public class QueryGroupStrategy : ISourcesSelector
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly string _groupAlias;

        public string QueryWithinDescription => $"group `{_groupAlias}`";

        public QueryGroupStrategy(ISettingsService settingsService, IPluginService pluginService, string groupAlias)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _groupAlias = groupAlias;
        }

        public IEnumerable<ISource> GetSources()
        {
            ArgumentException.ThrowIfNullOrEmpty(_groupAlias);

            var output = _settingsService
                .ReadSettingsFileSources()
                .Where(x => x.Groups?.Contains(_groupAlias) ?? false);

            if (!output.Any())
            {
                throw new GroupNotFoundException(_groupAlias);
            }

            return output.ToRealInstance(_pluginService);
        }
    }
}
