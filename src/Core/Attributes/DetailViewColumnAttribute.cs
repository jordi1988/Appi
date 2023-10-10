namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DetailViewColumnAttribute : Attribute
    {
        public Type TargetType { get; }

        public DetailViewColumnAttribute(Type? type = null)
        {
            TargetType = type ?? typeof(string);
        }
    }
}
