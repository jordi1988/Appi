namespace Domain.Entities
{
    public class ActionItem
    {
        public string Name { get; set; } = string.Empty;
        public Action Action { get; set; } = () => { };

        public override string ToString()
        {
            return Name;
        }
    }
}
