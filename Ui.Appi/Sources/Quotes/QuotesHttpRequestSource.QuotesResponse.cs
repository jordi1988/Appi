using Infrastructure.Sources.HttpRequest;

namespace Ui.Appi.Sources.Quotes
{
    internal partial class QuotesHttpRequestSource
    {
        internal sealed record QuotesResponse(IEnumerable<QuotesHttpRequestResult> Results);
    }
}
