using Core.Abstractions;

namespace Core.Extensions
{
    public static class SourceExtensions
    {
        public static void CopyTo(this ISource source, ISource instance)
        {
            instance.Name = source.Name;
            instance.Description = source.Description;
            instance.IsActive = true;
            instance.SortOrder = source.SortOrder;
            instance.Path = source.Path;
        }
    }
}
