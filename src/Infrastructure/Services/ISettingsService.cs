using Core.Abstractions;
using Core.Models;
using Infrastructure.Strategies;

namespace Infrastructure.Services
{
    public interface ISettingsService
    {
        ISourceServiceSelector CalculateStrategy(FindItemsOptions options, string queryAllDefaultValue);
        ISource CreateInstance(ISource source);
        IEnumerable<ISource> ReadSettingsFileSources();
        void SaveSettingsFileSources(IEnumerable<ISource> sources);
    }
}