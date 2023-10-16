namespace Core.Models
{
    /// <summary>
    /// Represents the user's query.
    /// </summary>
    public class FindItemsOptions
    {
        /// <summary>
        /// Gets or sets the typed in query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the typed in group alias.
        /// </summary>
        /// <value>
        /// The group alias.
        /// </value>
        public string? GroupAlias { get; set; }

        /// <summary>
        /// Gets or sets the typed in source alias.
        /// </summary>
        /// <value>
        /// The source alias.
        /// </value>
        public string? SourceAlias { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the query should be case sensitive.
        /// </summary>
        /// <value><c>true</c> if the user wishes the query to be case sensitive.</value>
        public bool CaseSensitive { get; set; }
    }
}
