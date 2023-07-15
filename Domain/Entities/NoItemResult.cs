using Domain.Interfaces;

namespace Domain.Entities
{
    /// <summary>
    /// Null object pattern
    /// </summary>
    public class NoItemResult : Result
    {
        public override IEnumerable<ActionItem> GetActions()
        {
            return Enumerable.Empty<ActionItem>();
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
