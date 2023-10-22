using Core.Abstractions;
using Core.Attributes;
using Core.Models;

namespace Infrastructure.ExternalSourceDemo
{
    public class ExternalDemoResult : ResultItemBase
    {
        private readonly IHandlerHelper? _handlerHelper;

        [DetailViewColumn]
        public override string Name => base.Name;

        [DetailViewColumn]
        public override string Description => _handlerHelper?.EscapeMarkup(base.Description) ?? base.Description;

        public ExternalDemoResult(IHandlerHelper? handlerHelper)
        {
            _handlerHelper = handlerHelper;
        }

        public override IEnumerable<ActionItem> GetActions()
        {
            if (_handlerHelper is null)
            {
                return Enumerable.Empty<ActionItem>();
            }

            var actions = new List<ActionItem>
            {
                _handlerHelper.Back(),
                _handlerHelper.Exit()
            };

            return actions;
        }

        public override string ToString()
        {
            return $"{Name,-30}{Description,-35}";
        }
    }
}
