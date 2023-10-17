using Core.Abstractions;
using Core.Models;

namespace Infrastructure.Sources.MoreOptions
{
    /// <summary>
    /// Represents the class for non-context item results.
    /// </summary>
    /// <seealso cref="ResultItemBase" />
    public sealed class MoreOptionsItemResult : ResultItemBase
    {
        private readonly IHandlerHelper _handlerHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreOptionsItemResult"/> class.
        /// </summary>
        /// <param name="handlerHelper">The handler helper.</param>
        /// <exception cref="ArgumentNullException">handlerHelper</exception>
        public MoreOptionsItemResult(IHandlerHelper handlerHelper)
        {
            _handlerHelper = handlerHelper ?? throw new ArgumentNullException(nameof(handlerHelper));

            Name = "More options";
            Description = string.Empty;
        }

        /// <inheritdoc cref="ResultItemBase.GetActions"/>
        public override IEnumerable<ActionItem> GetActions()
        {
            var actions = new List<ActionItem>
            {
                _handlerHelper.Back(),
                _handlerHelper.Exit()
            };

            return actions;
        }

        /// <inheritdoc cref="ResultItemBase.ToString"/>
        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
