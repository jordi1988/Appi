using Core.Abstractions;
using Core.Helper;

namespace Core.Extensions
{
    /// <summary>
    /// Extends the <see cref="ISource"/> interface.
    /// </summary>
    public static class SourceExtensions
    {
        /// <summary>
        /// Copies all properties from one instance to another.
        /// </summary>
        /// <param name="source">The source class.</param>
        /// <param name="destination">The destination class.</param>
        public static void CopyTo(this ISource source, ISource destination)
        {
            destination.Name = source.Name;
            destination.Alias = source.Alias;
            destination.Description = source.Description;
            destination.IsActive = source.IsActive;
            destination.SortOrder = source.SortOrder;
            destination.Path = source.Path;
            destination.Arguments = source.Arguments;
        }

        /// <summary>
        /// Creates a new instance of a manually created one.
        /// </summary>
        /// <param name="source">The manually created source.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <returns>An instance that can make use of the <see cref="ISource.ReadAsync(Models.FindItemsOptions)"/> method.</returns>
        public static ISource ToRealInstance(this ISource source, IServiceProvider serviceProvider)
        {
            var sourceClass = ReflectionHelper.GetClassByNameImplementingInterface<ISource>(source.TypeName, serviceProvider);
            var instance = ReflectionHelper.CreateInstance<ISource>(sourceClass, serviceProvider);

            source.CopyTo(instance);

            return instance;
        }

        /// <summary>
        /// Creates a new instance of a manually created one.
        /// </summary>
        /// <param name="sources">The sources.</param>
        /// <param name="serviceProvider">The service provider for accessing the registered services.</param>
        /// <returns>A collection of instances that can make use of the <see cref="ISource.ReadAsync(Models.FindItemsOptions)"/> method.</returns>
        public static IEnumerable<ISource> ToRealInstance(this IEnumerable<ISource> sources, IServiceProvider serviceProvider)
        {
            var output = new List<ISource>();

            foreach (var source in sources)
            {
                var instance = ToRealInstance(source, serviceProvider);

                output.Add(instance);
            }

            return output;
        }
    }
}
