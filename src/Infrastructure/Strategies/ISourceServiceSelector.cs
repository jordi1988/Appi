using Core.Abstractions;
using Core.Exceptions;
using Infrastructure.Extensions;
using Infrastructure.Services;

namespace Infrastructure.Strategies
{
    public interface ISourceServiceSelector
    {
        string QueryWithinDescription { get; }

        IEnumerable<ISource> GetSources();
    }

    public class QueryAllActiveSourcesStrategy : ISourceServiceSelector
    {
        private readonly FileSettingsService _sourceService;

        public string QueryWithinDescription => $"all active sources";

        public QueryAllActiveSourcesStrategy(FileSettingsService sourceService)
        {
            _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
        }

        public IEnumerable<ISource> GetSources()
        {
            var sources = _sourceService
                .ReadSettingsFileSources()
                .Where(x => x.IsActive);

            return sources.Instantiate(_sourceService);
        }
    }

    public class QueryGroupStrategy : ISourceServiceSelector
    {
        private readonly FileSettingsService _sourceService;
        private readonly string _groupAlias;

        public string QueryWithinDescription => $"group `{_groupAlias}`";

        public QueryGroupStrategy(FileSettingsService sourceService, string groupAlias)
        {
            _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
            _groupAlias = groupAlias;
        }

        public IEnumerable<ISource> GetSources()
        {
            ArgumentException.ThrowIfNullOrEmpty(_groupAlias);

            var output = _sourceService
                .ReadSettingsFileSources()
                .Where(x => x.Groups?.Contains(_groupAlias) ?? false);

            if (!output.Any())
            {
                throw new GroupNotFoundException(_groupAlias);
            }

            return output.Instantiate(_sourceService);
        }
    }

    public class QuerySingleSourceStrategy : ISourceServiceSelector
    {
        private readonly FileSettingsService _sourceService;
        private readonly string _sourceAlias;

        public string QueryWithinDescription => $"source `{_sourceAlias}`";

        public QuerySingleSourceStrategy(FileSettingsService sourceService, string sourceAlias)
        {
            _sourceService = sourceService ?? throw new ArgumentNullException(nameof(sourceService));
            _sourceAlias = sourceAlias;
        }

        public IEnumerable<ISource> GetSources()
        {
            var source = _sourceService
                .ReadSettingsFileSources()
                .FirstOrDefault(x => _sourceAlias!.Equals(x.Alias))
                    ?? throw new SourceNotFoundException($"{_sourceAlias} (Alias)");

            var output = _sourceService.CreateInstance(source);

            return new[] { output };
        }
    }
}
