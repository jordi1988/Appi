using Core.Abstractions;

namespace Core.Models
{
    /// <summary>
    /// Represents the parent item for grouping resulting items.
    /// </summary>
    /// <seealso cref="ResultItemBase" />
    public sealed class GroupHeaderResult : ResultItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupHeaderResult"/> class.
        /// </summary>
        /// <param name="name">The group's name.</param>
        /// <param name="description">The group's description.</param>
        public GroupHeaderResult(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets the actions.
        /// </summary>
        /// <returns>Empty collection of actions, because headers cannot be selected.</returns>
        public override IEnumerable<ActionItem> GetActions()
        {
            return Enumerable.Empty<ActionItem>();
        }

        /// <summary>
        /// Displays the group header.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Name} ({Description})";
        }
    }
}
