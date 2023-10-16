using Core.Models;

namespace Core.Abstractions
{
    /// <summary>
    /// Represents the base class for a result object.
    /// </summary>
    public abstract class ResultItemBase
    {
        /// <summary>
        /// Gets or sets the result's identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the result's name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the result's description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the result's sort order.
        /// </summary>
        /// <value>
        /// The sort.
        /// </value>
        public virtual int Sort { get; set; }

        /// <summary>
        /// Gets the actions that can be invoked if result is selected.
        /// </summary>
        /// <returns>All usable actions.</returns>
        /// <remarks>Make use of <see cref="IHandlerHelper.Back"/> and <see cref="IHandlerHelper.Exit"/> if you like to.</remarks>
        public abstract IEnumerable<ActionItem> GetActions();

        /// <summary>
        /// Overridden method that displays the result.
        /// </summary>
        public override abstract string ToString();
    }
}
