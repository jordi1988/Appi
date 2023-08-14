using Core.Abstractions;
using Core.Extensions;
using Core.Helper;
using Core.Models;
using System.Text.Json;
using System.Linq;

namespace Core.Services
{
    public class SourceService
    {
        public IEnumerable<ISource> GetActiveSources(IPluginService pluginService)
        {
            var output = new List<ISource>();

            var settingsFileActiveSources = ReadSettingsFileSources().Where(x => x.IsActive);
            foreach (var source in settingsFileActiveSources)
            {
                var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, pluginService);
                var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass);

                source.CopyTo(instance);

                output.Add(instance);
            }

            return output;
        }

        public IEnumerable<ISource> ReadSettingsFileSources()
        {
            ConfigurationHelper.EnsureSettingsExist();

            var fileContents = File.ReadAllText(ConfigurationHelper.ApplicationFilename);
            var fileSources = JsonSerializer.Deserialize<IEnumerable<EmptySource>>(fileContents);
            
            RepairFileSourcesAndSave(fileSources);

            return fileSources ?? Enumerable.Empty<ISource>();
        }

        private void RepairFileSourcesAndSave(IEnumerable<EmptySource>? fileSources)
        {
            if (fileSources is null || !fileSources.Any())
            {
                return;
            }

            bool wasRepaired = false;
            var sourcesToRepair = fileSources.Where(
                source => string.IsNullOrWhiteSpace(source.Alias));
            foreach (var source in sourcesToRepair)
            {
                source.Alias = source.Name;
                wasRepaired = true;
            }

            if (wasRepaired)
            {
                SaveSettingsFileSources(fileSources);
            }
        }

        public void SaveSettingsFileSources(IEnumerable<ISource> sources)
        {
            ConfigurationHelper.EnsureSettingsExist();

            var content = JsonSerializer.Serialize(sources, 
                new JsonSerializerOptions { 
                    WriteIndented = true 
                });

            File.WriteAllText(ConfigurationHelper.ApplicationFilename, content);
        }
    }
}
