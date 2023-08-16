using Core.Abstractions;
using Core.Exceptions;
using Core.Extensions;
using Core.Helper;
using Core.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Core.Services
{
    public partial class SourceService
    {
        private readonly IPluginService _pluginService;

        public SourceService(IPluginService pluginService)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
        }

        public IEnumerable<ISource> GetActiveSources()
        {
            var output = new List<ISource>();

            var settingsFileActiveSources = ReadSettingsFileSources().Where(x => x.IsActive);
            foreach (var source in settingsFileActiveSources)
            {
                var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, _pluginService);
                var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass);

                source.CopyTo(instance);

                output.Add(instance);
            }

            return output;
        }

        public ISource GetSourceByAlias(string alias)
        {
            // TODO: Check for duplicate aliases
            ArgumentException.ThrowIfNullOrEmpty(nameof(alias));

            var source = ReadSettingsFileSources().FirstOrDefault(x => alias.Equals(x.Alias));
            if (source is null)
            {
                throw new SourceNotFoundException($"{alias} (Alias)");
            }

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

        // [GeneratedRegex("[^\u0000-\u007F]")]
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
                          source.Alias.Equals("all") ||
                          source.IsQueryCommand is null);

            // TODO: duplicate aliases are illegal

            if (!sourcesToRepair.Any() )
            {
                return;
            }

            foreach (var source in sourcesToRepair)
            {
                source.Alias = AsciiCharactersOnlyRegex()
                    .Replace(source.Name, string.Empty)
                    .ToLowerInvariant();

                source.IsQueryCommand ??= true;
            }

            SaveSettingsFileSources(fileSources);
        }
    }
}
