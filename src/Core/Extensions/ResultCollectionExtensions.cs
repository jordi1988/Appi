using Core.Abstractions;

namespace Core.Extensions
{
    public static class ResultCollectionExtensions
    {
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
