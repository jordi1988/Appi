using Core.Abstractions;
using Core.Exceptions;
using Core.Extensions;

namespace Core.Strategies
{
    public class QuerySingleSourceStrategy : ISourcesSelector
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly string _sourceAlias;

        public string QueryWithinDescription => $"source `{_sourceAlias}`";

        public QuerySingleSourceStrategy(ISettingsService settingsService, IPluginService pluginService, string sourceAlias)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _sourceAlias = sourceAlias;
        }

        public IEnumerable<ISource> GetSources()
        {
            var source = _settingsService
                .ReadSettingsFileSources()
                .FirstOrDefault(x => _sourceAlias!.Equals(x.Alias))
                    ?? throw new SourceNotFoundException($"{_sourceAlias} (Alias)");

            var output = new[] { source };

            return output.Instantiate(_pluginService);
        }
    }
}
