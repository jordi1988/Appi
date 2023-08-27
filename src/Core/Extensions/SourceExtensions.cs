using Core.Abstractions;
using Core.Helper;

namespace Core.Extensions
{
    public static class SourceExtensions
    {
        public static void CopyTo(this ISource source, ISource instance)
        {
            instance.Name = source.Name;
            instance.Alias = source.Alias;
            instance.Description = source.Description;
            instance.IsActive = source.IsActive;
            instance.SortOrder = source.SortOrder;
            instance.Path = source.Path;
            instance.Arguments = source.Arguments;
        }

        public static ISource CreateInstance(this ISource source, IPluginService pluginService)
        {
            var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, pluginService);
            var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass);

            source.CopyTo(instance);

            return instance;
        }

        public static IEnumerable<ISource> Instantiate(this IEnumerable<ISource> sources, IPluginService pluginService)
        {
            var output = new List<ISource>();

            foreach (var source in sources)
            {
                var instance = CreateInstance(source, pluginService);

                output.Add(instance);
            }

            return output;
        }
    }
}
