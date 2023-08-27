namespace Core.Exceptions
{
    public class GroupNotFoundException : CoreException
    {
        public GroupNotFoundException(string groupAlias)
            : base($"The provided group `{groupAlias}` does not contain any sources.")
        {
        }
    }
}
