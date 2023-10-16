namespace Core.Models
{
    /// <summary>
    /// Represents an item that can contain actions.
    /// </summary>
    public sealed class ActionItem
    {
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action of the item.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public Action Action { get; set; } = () => { };

        /// <summary>
        /// Overridden method for displaying the item´'s name.
        /// </summary>
        /// <returns>
        /// The name of the item.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
