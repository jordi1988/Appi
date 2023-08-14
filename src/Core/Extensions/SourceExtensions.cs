using Core.Abstractions;

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
    }
}
