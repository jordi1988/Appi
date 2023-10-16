using Core.Models;

namespace Core.Abstractions
{
    /// <summary>
    /// Represents an interface of a source that is described through properties and that can be read.
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type. Typically nameof(ImplementedTypeSource).
        /// </value>
        string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the displayed name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the displayed description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the alias to be used for a single source query.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        /// <remarks>Should consist of a single word.</remarks>
        string Alias { get; set; }

        /// <summary>
        /// Gets or sets the groups to be used for a group source query.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        string[]? Groups { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the source is active and is included when querying all sources.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the source is active and included in all sources query; otherwise, <c>false</c>.
        /// </value>
        bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the sort order of the source if multiple sources get displayed.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets any path as an argument for the source.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        /// <remarks>Each concrete implementation can use this property differently or not at all.</remarks>
        string? Path { get; set; }

        /// <summary>
        /// Gets or sets any arguments as an argument for the source.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        /// <remarks>
        /// Each concrete implementation can use this property differently or not at all.  
        /// Make use of <see href="https://github.com/jordi1988/ArgumentString">ArgumentString</see> (included in Appi.Infrastructure) if you like to.
        /// </remarks>
        string? Arguments { get; set; }

        /// <summary>
        /// Deterines if the source is recognized as normal query.
        /// </summary>
        /// <value>
        /// The is query command.
        /// </value>
        bool? IsQueryCommand { get; set; }

        /// <summary>
        /// Reads the source.
        /// </summary>
        /// <param name="options">Options related to the query.</param>
        /// <returns>The results of the query.</returns>
        Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options);
    }
}
