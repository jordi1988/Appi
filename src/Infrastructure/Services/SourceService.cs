using Core.Abstractions;
using Core.Extensions;
using Core.Helper;
using Core.Models;
using Infrastructure.Strategies;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Infrastructure.Services
{
    public partial class FileSettingsService : ISettingsService
    {
        private readonly IPluginService _pluginService;

        public FileSettingsService(IPluginService pluginService)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
        }

        public ISource CreateInstance(ISource source)
        {
            var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, _pluginService);
            var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass);

            source.CopyTo(instance);

            return instance;
        }

        public IEnumerable<ISource> ReadSettingsFileSources()
        {
            ConfigurationHelper.EnsureSettingsExist();

            var fileContents = File.ReadAllText(ConfigurationHelper.ApplicationFilename);
            var fileSources = JsonSerializer.Deserialize<IEnumerable<EmptySource>>(fileContents);

            RepairFileSourcesAndSave(fileSources);

            return fileSources ?? Enumerable.Empty<ISource>();
        }

        public void SaveSettingsFileSources(IEnumerable<ISource> sources)
        {
            ConfigurationHelper.EnsureSettingsExist();

            var content = JsonSerializer.Serialize(sources,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(ConfigurationHelper.ApplicationFilename, content);
        }

        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex AsciiCharactersOnlyRegex();

        private void RepairFileSourcesAndSave(IEnumerable<EmptySource>? fileSources)
        {
            if (fileSources is null || !fileSources.Any())
            {
                return;
            }

            var sourcesToRepair = fileSources.Where(
                source => string.IsNullOrWhiteSpace(source.Alias) ||
                          source.Alias.Contains(' ') ||
                          source.IsQueryCommand is null ||
                          source.Groups is null);

            if (!sourcesToRepair.Any())
            {
                return;
            }

            foreach (var source in sourcesToRepair)
            {
                source.Alias = AsciiCharactersOnlyRegex()
                    .Replace(source.Name, string.Empty)
                    .ToLowerInvariant();

                source.IsQueryCommand ??= true;
                source.Groups ??= Array.Empty<string>();
            }

            SaveSettingsFileSources(fileSources);
        }

        public ISourceServiceSelector CalculateStrategy(FindItemsOptions options, string queryAllDefaultValue)
        {
            bool isSourceProvided = !string.IsNullOrWhiteSpace(options.SourceAlias);
            bool isGroupProvided = !queryAllDefaultValue.Equals(options.GroupAlias);

            if (isSourceProvided)
            {
                return new QuerySingleSourceStrategy(this, options.SourceAlias!);
            }
            else if (isGroupProvided)
            {
                return new QueryGroupStrategy(this, options.GroupAlias!);
            }

            return new QueryAllActiveSourcesStrategy(this);
        }
    }
}
