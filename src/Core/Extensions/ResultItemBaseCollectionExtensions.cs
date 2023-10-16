using Core.Abstractions;

namespace Core.Extensions
{
    /// <summary>
    /// Extends collections of type <see cref="ResultItemBase"/>.
    /// </summary>
    public static class ResultItemBaseCollectionExtensions
    {
        /// <summary>
        /// Sorts the results comparing the <see cref="ResultItemBase.Sort"/> property.
        /// </summary>
        /// <param name="sourceResults">The sorted results.</param>
        /// <returns></returns>
        public static IEnumerable<ResultItemBase> SortResults(this IEnumerable<ResultItemBase>? sourceResults)
        {
            if (sourceResults is null)
            {
                return Enumerable.Empty<ResultItemBase>();
            }

            if (sourceResults.Any(x => x.Sort > 0))
            {
                sourceResults = sourceResults.OrderBy(x => x.Sort);
            }

            return sourceResults;
        }
    }
}
