namespace Core.Exceptions
{
    /// <summary>
    /// Represents the <see cref="GroupNotFoundException"/> class which will be used if a given group alias name is not found.
    /// </summary>
    /// <seealso cref="CoreException" />
    public class GroupNotFoundException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupNotFoundException"/> class.
        /// </summary>
        /// <param name="groupAlias">The missing group alias name.</param>
        public GroupNotFoundException(string groupAlias)
            : base($"The provided group `{groupAlias}` does not contain any sources.")
        {
        }
    }
}
