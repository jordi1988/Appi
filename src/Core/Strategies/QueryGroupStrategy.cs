using Core.Abstractions;
using Core.Exceptions;
using Core.Extensions;

namespace Core.Strategies
{
    /// <summary>
    /// Represents the strategy for querying all sources of a given group.
    /// </summary>
    /// <seealso cref="ISourcesSelector" />
    public sealed class QueryGroupStrategy : ISourcesSelector
    {
        private readonly ISettingsService _settingsService;
        private readonly IPluginService _pluginService;
        private readonly IHandlerHelper _handlerHelper;
        private readonly string _groupAlias;

        /// <inheritdoc cref="ISourcesSelector.QueryWithinDescription"/>
        public string QueryWithinDescription => $"group `{_groupAlias}`";

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryGroupStrategy"/> class.
        /// </summary>
        /// <param name="settingsService">The settings service.</param>
        /// <param name="pluginService">The plugin service.</param>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <param name="groupAlias">The group alias.</param>
        /// <exception cref="System.ArgumentNullException">
        /// settingsService
        /// or
        /// pluginService
        /// or
        /// handlerHelper
        /// </exception>
        public QueryGroupStrategy(ISettingsService settingsService, IPluginService pluginService, IHandlerHelper handlerHelper, string groupAlias)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));
            _groupAlias = groupAlias;
        }

        /// <inheritdoc cref="ISourcesSelector.GetSources"/>
        public IEnumerable<ISource> GetSources()
        {
            ArgumentException.ThrowIfNullOrEmpty(_groupAlias);

            var output = _settingsService
                .ReadSources()
                .Where(x => x.Groups?.Contains(_groupAlias) ?? false);

            if (!output.Any())
            {
                throw new GroupNotFoundException(_groupAlias);
            }

            return output.ToRealInstance(_pluginService, _handlerHelper);
        }
    }
}
