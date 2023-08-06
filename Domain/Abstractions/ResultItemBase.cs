using Core.Entities;

namespace Core.Abstractions
{
    public abstract class ResultItemBase
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; } = string.Empty;
        public virtual string Description { get; set; } = string.Empty;
        public virtual int Sort { get; set; }


        public abstract IEnumerable<ActionItem> GetActions();

        public override abstract string ToString();
    }
}
