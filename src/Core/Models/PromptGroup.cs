using Core.Abstractions;

namespace Core.Models
{
    /// <summary>
    /// Represents a container class when displaying a promt.
    /// </summary>
    public sealed class PromptGroup
    {
        /// <summary>
        /// Gets or sets the promt group's name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the promt group's description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the promt group's items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IEnumerable<ResultItemBase> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromptGroup"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="items">The items.</param>
        public PromptGroup(string name, string description, IEnumerable<ResultItemBase> items)
        {
            Name = name;
            Description = description;
            Items = items;
        }
    }
}
