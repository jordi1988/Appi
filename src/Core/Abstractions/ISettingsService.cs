using Core.Models;

namespace Core.Abstractions
{
    public interface ISettingsService
    {
        IEnumerable<ISource> ReadSettingsFileSources();

        void SaveSettingsFileSources(IEnumerable<ISource> sources);
    }
}
