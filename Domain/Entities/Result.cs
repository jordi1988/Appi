namespace Domain.Entities
{
    public abstract class Result
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; } = string.Empty;
        public virtual string Description { get; set; } = string.Empty;

        public abstract IEnumerable<ActionItem> GetActions();

        public override abstract string ToString();
    }
}
