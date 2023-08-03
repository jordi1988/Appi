using Domain.Entities;

namespace Ui.Appi.Helper
{
    internal static class ResultCollectionHelper
    {
        public static IEnumerable<Result> SortResults(this IEnumerable<Result>? sourceResults)
        {
            if (sourceResults is null)
            {
                return Enumerable.Empty<Result>();
            }

            if (sourceResults.Any(x => x.Sort > 0))
            {
                sourceResults = sourceResults.OrderBy(x => x.Sort);
            }

            return sourceResults;
        }
    }
}
