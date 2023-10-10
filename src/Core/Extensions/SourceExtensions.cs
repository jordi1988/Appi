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

        public static ISource ToRealInstance(this ISource source, IPluginService pluginService, IHandlerHelper handlerHelper)
        {
            var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, pluginService);
            var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass, handlerHelper);

            source.CopyTo(instance);

            return instance;
        }

        public static IEnumerable<ISource> ToRealInstance(this IEnumerable<ISource> sources, IPluginService pluginService, IHandlerHelper handlerHelper)
        {
            var output = new List<ISource>();

            foreach (var source in sources)
            {
                var instance = ToRealInstance(source, pluginService, handlerHelper);

                output.Add(instance);
            }

            return output;
        }
    }
}
