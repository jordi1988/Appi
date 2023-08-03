namespace Domain.Entities
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ResultAttribute : Attribute
    {
        public Type TargetType { get; }

        public ResultAttribute(Type? type = null)
        {
            TargetType = type ?? typeof(string);
        }
    }
}
