using Core.Abstractions;
using Infrastructure.Services;

namespace Infrastructure.Extensions
{
    public static class SourceCollectionExtensions
    {
        public static IEnumerable<ISource> Instantiate(this IEnumerable<ISource> sources, FileSettingsService sourceService)
        {
            var output = new List<ISource>();

            foreach (var source in sources)
            {
                var instance = sourceService.CreateInstance(source);

                output.Add(instance);
            }

            return output;
        }
    }
}
