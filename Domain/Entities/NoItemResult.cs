using Core.Abstractions;

namespace Core.Entities
{
    /// <summary>
    /// Null object pattern
    /// </summary>
    public class NoItemResult : ResultItemBase
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
