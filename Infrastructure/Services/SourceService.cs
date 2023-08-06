using Core.Abstractions;
using Core.Entities;
using Core.Extensions;
using Core.Helper;
using System.Text.Json;

namespace Core.Services
{
    public class SourceService
    {
        public IEnumerable<ISource> GetActiveSources(object? settings, IPluginService pluginService)
        {
            var output = new List<ISource>();

            var settingsFileActiveSources = ReadSettingsFileSources().Where(x => x.IsActive);
            foreach (var source in settingsFileActiveSources)
            {
                var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, pluginService);
                var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass, settings);

                source.CopyTo(instance);

                output.Add(instance);
            }

            return output;
        }

        public IEnumerable<ISource> ReadSettingsFileSources()
        {
            ConfigurationHelper.EnsureSettingsExist();

            var fileContents = File.ReadAllText(ConfigurationHelper.ApplicationFilename);
            var fileSources = JsonSerializer.Deserialize<IEnumerable<DeserializationSource>>(fileContents);

            return fileSources ?? Enumerable.Empty<ISource>();
        }

        public void SaveSettingsFileSources(IEnumerable<ISource> sources)
        {
            ConfigurationHelper.EnsureSettingsExist();

            var content = JsonSerializer.Serialize(sources, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(ConfigurationHelper.ApplicationFilename, content);
        }
    }
}
